/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class can be used to view a playing area.
	/// </summary>
	sealed class PlayingAreaViewer : VisibleEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		private readonly Camera m_camera;

		/// <summary>
		/// The entity currently being placed by the user (if any).
		/// </summary>
		private IPlaceableEntity m_entityToPlace;

		/// <summary>
		/// The current projection matrix.
		/// </summary>
		private Matrix m_matProjection;

		/// <summary>
		/// The current view matrix.
		/// </summary>
		private Matrix m_matView;

		/// <summary>
		/// The current world matrix.
		/// </summary>
		private Matrix m_matWorld;

		/// <summary>
		/// The orientation of the entity currently being placed (if any).
		/// </summary>
		private Orientation4 m_placementOrientation = Orientation4.XPOS;

		/// <summary>
		/// The playing area to view.
		/// </summary>
		private readonly PlayingArea m_playingArea;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a playing area viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the viewer's XML representation.</param>
		public PlayingAreaViewer(XElement entityElt)
		:	base(entityElt)
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), Vector3.UnitZ);
			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			Viewport = Properties["Viewport"];
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the playing area.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Viewport;
			Renderer.Setup3D();
			SetupMatrices();

			DrawPlayingArea((dynamic)m_playingArea);
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			if(m_playingArea.BlueprintToPlace != null)
			{
				// Find the point we're hovering over on the near clipping plane.
				Vector3 near = Viewport.Unproject(new Vector3(state.X, state.Y, 0), m_matProjection, m_matView, m_matWorld);

				// Find the point we're hovering over on the far clipping plane.
				Vector3 far = Viewport.Unproject(new Vector3(state.X, state.Y, 1), m_matProjection, m_matView, m_matWorld);

				// Find the ray (in world space) between them.
				Vector3 dir = Vector3.Normalize(far - near);
				var ray = new Ray(near, dir);

				// Determine which grid square we're hovering over (if any).
				Vector2i? gridSquare = m_playingArea.Terrain.PickGridSquare(ray);

				m_entityToPlace = null;
				if(gridSquare != null && (m_playingArea.BlueprintToPlace == "Dwelling" || m_playingArea.BlueprintToPlace == "Mansion"))
				{
					// Work out what type of entity we're trying to place.
					Blueprint blueprint = SceneGraph.GetEntityByPath("blueprints/" + m_playingArea.BlueprintToPlace);
					Type entityType = blueprint.EntityType;

					// Attempt to determine the average altitude of the terrain beneath the entity's footprint.
					// Note that this will return null if the entity can't be validly placed.
					Footprint footprint = blueprint.Footprint.Rotated((int)m_placementOrientation);
					float? altitude = footprint.DetermineAverageAltitude(gridSquare.Value, m_playingArea.Terrain);

					// Provided the altitude could be determined, continue with entity creation.
					if(altitude != null)
					{
						// Set the properties of the entity.
						var entityProperties = new Dictionary<string,dynamic>();
						entityProperties["Altitude"] = altitude;
						entityProperties["Blueprint"] = m_playingArea.BlueprintToPlace;
						entityProperties["Orientation"] = m_placementOrientation;
						entityProperties["Position"] = gridSquare.Value;

						// Create the new entity, and set it as the entity to be placed if it's valid.
						IPlaceableEntity entityToPlace = Activator.CreateInstance(entityType, entityProperties, EntityStateID.OPERATING) as IPlaceableEntity;
						if(entityToPlace.IsValidlyPlaced(m_playingArea.Terrain))
						{
							m_entityToPlace = entityToPlace;
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			if(state.LeftButton == ButtonState.Pressed)
			{
				if(m_entityToPlace != null)
				{
					m_playingArea.AddEntityDynamic(m_entityToPlace.CloneNew());
					m_playingArea.BlueprintToPlace = null;
					m_entityToPlace = null;
				}
			}
			else if(state.RightButton == ButtonState.Pressed)
			{
				int placementOrientation = (int)m_placementOrientation;
				placementOrientation = (placementOrientation + 1) % 4;
				m_placementOrientation = (Orientation4)placementOrientation;

				// Call the mouse moved handler to update the entity being placed.
				OnMouseMoved(state);
			}
		}

		/// <summary>
		/// Updates the viewer based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Determine the linear, horizontal angular, and vertical angular movement rates.
			float[,] heightmap = m_playingArea.Terrain.Heightmap;
			float scalingFactor = Math.Max(heightmap.GetLength(0), heightmap.GetLength(1));
			float linearRate = 0.0005f * scalingFactor * gameTime.ElapsedGameTime.Milliseconds;
			float angularRateH = 0.002f * gameTime.ElapsedGameTime.Milliseconds;	// in radians
			float angularRateV = 0.0015f * gameTime.ElapsedGameTime.Milliseconds;	// in radians

			// Alter the camera based on user input.
			KeyboardState keyState = Keyboard.GetState();
			if(keyState.IsKeyDown(Keys.W))		m_camera.MoveN(linearRate);
			if(keyState.IsKeyDown(Keys.S))		m_camera.MoveN(-linearRate);
			if(keyState.IsKeyDown(Keys.A))		m_camera.MoveU(linearRate);
			if(keyState.IsKeyDown(Keys.D))		m_camera.MoveU(-linearRate);
			if(keyState.IsKeyDown(Keys.Q))		m_camera.MoveV(linearRate);
			if(keyState.IsKeyDown(Keys.E))		m_camera.MoveV(-linearRate);
			if(keyState.IsKeyDown(Keys.Left))	m_camera.Rotate(Vector3.UnitZ, angularRateH);
			if(keyState.IsKeyDown(Keys.Right))	m_camera.Rotate(Vector3.UnitZ, -angularRateH);
			if(keyState.IsKeyDown(Keys.Up))		m_camera.Rotate(m_camera.U, angularRateV);
			if(keyState.IsKeyDown(Keys.Down))	m_camera.Rotate(m_camera.U, -angularRateV);

			if(keyState.IsKeyDown(Keys.LeftShift))
			{
				MouseState mouseState = Mouse.GetState();
				float xOffset = Viewport.X + Viewport.Width * 0.5f - mouseState.X;
				float yOffset = mouseState.Y - (Viewport.Y + Viewport.Height * 0.5f);
				if(Math.Abs(xOffset) > Viewport.Width / 8)
				{
					m_camera.Rotate(Vector3.UnitZ, xOffset * 0.000005f * gameTime.ElapsedGameTime.Milliseconds);
				}
				if(Math.Abs(yOffset) > Viewport.Height / 8)
				{
					m_camera.Rotate(m_camera.U, yOffset * 0.000005f * gameTime.ElapsedGameTime.Milliseconds);
				}
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws a placeable entity.
		/// </summary>
		/// <param name="entity">The entity to draw.</param>
		private void DrawPlaceableEntity(IPlaceableEntity entity)
		{
			// Load the model.
			Model model = Renderer.Content.Load<Model>("Models/" + entity.Blueprint.Model);

			// Move the model to the correct position.
			Matrix matWorld = Matrix.CreateTranslation(entity.Position.X + 0.5f, entity.Position.Y + 0.5f, entity.Altitude);

			// If the entity has a non-default orientation, rotate the model appropriately.
			if(entity.Orientation != Orientation4.XPOS)
			{
				float angle = Convert.ToInt32(entity.Orientation) * MathHelper.PiOver2;
				Matrix matRot = Matrix.CreateRotationZ(angle);
				matWorld = Matrix.Multiply(matRot, matWorld);
			}

			// If the entity is still being constructed, scale the model based on the current state of completion.
			if(entity.FSM.CurrentStateID == EntityStateID.IN_CONSTRUCTION)
			{
				Matrix matScale = Matrix.CreateScale(1, 1, entity.FSM.CurrentState.PercentComplete / 100f);
				matWorld = Matrix.Multiply(matScale, matWorld);
			}

			// Render the model.
			Renderer.DrawModel(model, matWorld, m_matView, m_matProjection);
		}

		/// <summary>
		/// Draws the playing area for a city.
		/// </summary>
		/// <param name="city">The city.</param>
		private void DrawPlayingArea(City city)
		{
			// Draw the city's underlying terrain.
			DrawTerrain(city.Terrain);

			// For debugging purposes only.
			//DrawTerrainQuadtree(city.Terrain.QuadtreeRoot);

			// Draw all the buildings in the city.
			foreach(Building building in city.Buildings)
			{
				DrawPlaceableEntity(building);
			}

			// Draw the entity currently being placed (if any).
			if(m_entityToPlace != null)
			{
				DrawPlaceableEntity(m_entityToPlace);
			}
		}

		/// <summary>
		/// Draws the playing area for the world.
		/// </summary>
		/// <param name="world">The world.</param>
		private void DrawPlayingArea(World world)
		{
			// Draw the world's underlying terrain.
			DrawTerrain(world.Terrain);

			// For debugging purposes only.
			//DrawTerrainQuadtree(world.Terrain.QuadtreeRoot);
		}

		/// <summary>
		/// Draws a terrain.
		/// </summary>
		/// <param name="terrain">The terrain to draw.</param>
		private void DrawTerrain(Terrain terrain)
		{
			var effect = Renderer.Content.Load<Effect>("Effects/TerrainMultitexture");
			effect.Parameters["World"].SetValue(m_matWorld);
			effect.Parameters["View"].SetValue(m_matView);
			effect.Parameters["Projection"].SetValue(m_matProjection);
			effect.Parameters["Texture0"].SetValue(Renderer.Content.Load<Texture2D>("Textures/grass"));
			effect.Parameters["Texture1"].SetValue(Renderer.Content.Load<Texture2D>("Textures/snow"));
			effect.Parameters["TransitionHalfWidth"].SetValue(4f);
			effect.Parameters["TransitionHeight"].SetValue(10f);
			Renderer.DrawTriangleList(terrain.VertexBuffer, terrain.IndexBuffer, effect);
		}

		/// <summary>
		/// Draws the bounding boxes of the various nodes in a terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="root">The root node of the terrain quadtree to draw.</param>
		private void DrawTerrainQuadtree(QuadtreeNode root)
		{
			var basicEffect = new BasicEffect(Renderer.GraphicsDevice);
			basicEffect.World = m_matWorld;
			basicEffect.View = m_matView;
			basicEffect.Projection = m_matProjection;
			basicEffect.VertexColorEnabled = true;

			DrawTerrainQuadtreeSub(root, basicEffect);
		}

		/// <summary>
		/// Draws the bounding boxes of the various nodes in a subtree of the terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="node">The root node of the subtree.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="depth">The depth of the recursion.</param>
		private void DrawTerrainQuadtreeSub(QuadtreeNode node, Effect effect, int depth = 0)
		{
			if(node.Children != null)
			{
				// This node is a branch, so recurse on its children.
				foreach(QuadtreeNode child in node.Children)
				{
					DrawTerrainQuadtreeSub(child, effect, depth + 1);
				}
			}

			// Draw the node's own bounding box.
			var colours = new Color[] { Color.Cyan, Color.Yellow, Color.Magenta };
			Renderer.DrawBoundingBox(node.Bounds, effect, colours[depth % colours.Length]);
		}

		/// <summary>
		/// Sets up the world, view and projection matrices ready for rendering.
		/// </summary>
		private void SetupMatrices()
		{
			m_matProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)Viewport.Width / Viewport.Height, 0.1f, 1000.0f);
			m_matView = Matrix.CreateLookAt(m_camera.Position, m_camera.Position + m_camera.N, m_camera.V);
			m_matWorld = Matrix.Identity;
		}

		#endregion
	}
}
