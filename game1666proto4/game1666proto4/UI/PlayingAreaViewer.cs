/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using game1666proto4.Common.Graphics;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Entities;
using game1666proto4.GameModel.FSMs;
using game1666proto4.UI.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
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
		/// The playing area to view.
		/// </summary>
		private readonly IPlayingArea m_playingArea;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The state shared between the visible entities that together make up the game view - e.g. things like the currently active tool.
		/// </summary>
		public GameViewState GameViewState { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a playing area viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the viewer's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public PlayingAreaViewer(XElement entityElt, World world)
		:	base(entityElt, world)
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), Vector3.UnitZ);
			m_playingArea = World.GetEntityByPath(Properties["PlayingArea"]);
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

			DrawPlayingArea(m_playingArea);
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			if(GameViewState.Tool != null)
			{
				GameViewState.Tool.OnMouseMoved(state, Viewport, m_matProjection, m_matView, m_matWorld);
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			if(GameViewState.Tool != null)
			{
				GameViewState.Tool = GameViewState.Tool.OnMousePressed(state, Viewport, m_matProjection, m_matView, m_matWorld);
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
		/// <param name="alpha">The alpha value to use for the model when drawing.</param>
		private void DrawPlaceableEntity(IPlaceableEntity entity, float alpha = 1f)
		{
			// Determine the model name and orientation to use - this is a hook so that we can override
			// the default behaviour when drawing things like road segments. For most entities, we use
			// the default implementation, which just returns the entity's model name and orientation.
			Tuple<string,Orientation4> result = EntityUtil.DetermineModelNameAndOrientation((dynamic)entity, m_playingArea.OccupancyMap);
			string modelName = result.Item1;
			Orientation4 orientation = result.Item2;

			// Load the model.
			Model model = Renderer.Content.Load<Model>("Models/" + modelName);

			// Move the model to the correct position.
			Matrix matWorld = Matrix.CreateTranslation(entity.Position.X + 0.5f, entity.Position.Y + 0.5f, entity.Altitude);

			// If the entity has a non-default orientation, rotate the model appropriately.
			if(orientation != Orientation4.XPOS)
			{
				float angle = Convert.ToInt32(orientation) * MathHelper.PiOver2;
				Matrix matRot = Matrix.CreateRotationZ(angle);
				matWorld = Matrix.Multiply(matRot, matWorld);
			}

			// If the entity is being constructed or destructed, scale the model based on the current state of completion.
			if(entity.FSM.CurrentStateID == PlaceableEntityStateID.IN_CONSTRUCTION ||
			   entity.FSM.CurrentStateID == PlaceableEntityStateID.IN_DESTRUCTION)
			{
				Matrix matScale = Matrix.CreateScale(1, 1, entity.FSM.CurrentState.PercentComplete / 100f);
				matWorld = Matrix.Multiply(matScale, matWorld);
			}

			// Render the model.
			Renderer.DrawModel(model, matWorld, m_matView, m_matProjection, alpha);
		}

		/// <summary>
		/// Draws a playing area.
		/// </summary>
		/// <param name="playingArea">The playing area.</param>
		private void DrawPlayingArea(IPlayingArea playingArea)
		{
			// Draw the playing area's underlying terrain.
			DrawTerrain(playingArea.Terrain);

			// For debugging purposes only.
			//DrawTerrainQuadtree(playingArea.Terrain.QuadtreeRoot);

			// Draw all the entities in the playing area, making sure that if one is
			// liable to be deleted, we render it slightly transparently.
			ITool tool = GameViewState.Tool;
			foreach(IPlaceableEntity entity in playingArea.Children)
			{
				float alpha = tool != null && tool.Name == "Delete:Delete" && entity == tool.Entity ? 0.35f : 1f;
				DrawPlaceableEntity(entity, alpha);
			}

			// Draw the entity (if any) associated with the active tool if we're placing an entity.
			if(tool != null && tool.Name.StartsWith("Place:") && tool.Entity != null)
			{
				IPlaceableEntity entity = tool.Entity;
				float alpha = playingArea.OccupancyMap.IsValidlyPlaced(entity) ? 1f : 0.35f;
				DrawPlaceableEntity(entity, alpha);
			}
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
