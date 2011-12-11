/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
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
	sealed class PlayingAreaViewer : ViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		private readonly Camera m_camera;

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

			m_camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), new Vector3(0,0,1));
			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			Viewport = ViewUtil.ParseViewportSpecifier(Properties["Viewport"]);
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
			BasicEffect basicEffect = CreateBasicEffect();

			DrawTerrain(basicEffect);

			// For debugging purposes only.
			//DrawTerrainQuadtree(basicEffect);
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			if(state.LeftButton == ButtonState.Pressed)
			{
				BasicEffect viewerBasicEffect = CreateBasicEffect();

				// Find the point we're hovering over on the near clipping plane.
				Vector3 near = Viewport.Unproject(new Vector3(state.X, state.Y, 0), viewerBasicEffect.Projection, viewerBasicEffect.View, viewerBasicEffect.World);

				// Find the point we're hovering over on the far clipping plane.
				Vector3 far = Viewport.Unproject(new Vector3(state.X, state.Y, 1), viewerBasicEffect.Projection, viewerBasicEffect.View, viewerBasicEffect.World);

				// Find the ray (in world space) between them.
				Vector3 dir = Vector3.Normalize(far - near);
				var ray = new Ray(near, dir);

				// Output the grid square clicked by the user.
				Vector2i? gridSquare = m_playingArea.Terrain.PickGridSquare(ray);
				if(gridSquare != null)	System.Console.WriteLine(gridSquare.Value.X.ToString() + ' ' + gridSquare.Value.Y.ToString());
				else					System.Console.WriteLine("No grid square was clicked");
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
			if(keyState.IsKeyDown(Keys.Left))	m_camera.Rotate(new Vector3(0,0,1), angularRateH);
			if(keyState.IsKeyDown(Keys.Right))	m_camera.Rotate(new Vector3(0,0,1), -angularRateH);
			if(keyState.IsKeyDown(Keys.Up))		m_camera.Rotate(m_camera.U, angularRateV);
			if(keyState.IsKeyDown(Keys.Down))	m_camera.Rotate(m_camera.U, -angularRateV);

			MouseState mouseState = Mouse.GetState();
			if(mouseState.RightButton == ButtonState.Pressed)
			{
				float xOffset = Viewport.X + Viewport.Width * 0.5f - mouseState.X;
				float yOffset = mouseState.Y - (Viewport.Y + Viewport.Height * 0.5f);
				if(Math.Abs(xOffset) > Viewport.Width / 8)
				{
					m_camera.Rotate(new Vector3(0,0,1), xOffset * 0.000005f * gameTime.ElapsedGameTime.Milliseconds);
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
		/// Creates the underlying basic effect for the viewer as a whole (ready to be customised).
		/// </summary>
		/// <returns>The basic effect.</returns>
		private BasicEffect CreateBasicEffect()
		{
			var basicEffect = new BasicEffect(Renderer.GraphicsDevice);
			basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)Viewport.Width / Viewport.Height, 0.1f, 1000.0f);
			basicEffect.View = Matrix.CreateLookAt(m_camera.Position, m_camera.Position + m_camera.N, m_camera.V);
			basicEffect.World = Matrix.Identity;
			return basicEffect;
		}

		/// <summary>
		/// Draws the playing area's terrain.
		/// </summary>
		/// <param name="viewerBasicEffect">The underlying basic effect for the viewer as a whole (ready to be customised).</param>
		private void DrawTerrain(BasicEffect viewerBasicEffect)
		{
			BasicEffect basicEffect = viewerBasicEffect.Clone() as BasicEffect;
			basicEffect.Texture = Renderer.Content.Load<Texture2D>("grass");
			basicEffect.TextureEnabled = true;
			Renderer.DrawTriangleList(m_playingArea.Terrain.VertexBuffer, m_playingArea.Terrain.IndexBuffer, basicEffect);
		}

		/// <summary>
		/// Draws the bounding boxes of the various nodes in the terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="viewerBasicEffect">The underlying basic effect for the viewer as a whole (ready to be customised).</param>
		private void DrawTerrainQuadtree(BasicEffect viewerBasicEffect)
		{
			BasicEffect basicEffect = viewerBasicEffect.Clone() as BasicEffect;
			basicEffect.VertexColorEnabled = true;
			DrawTerrainQuadtreeSub(m_playingArea.Terrain.QuadtreeRoot, basicEffect);
		}

		/// <summary>
		/// Draws the bounding boxes of the various nodes in a subtree of the terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="node">The root node of the subtree.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="depth">The depth of the recursion.</param>
		private void DrawTerrainQuadtreeSub(QuadtreeNode node, BasicEffect basicEffect, int depth = 0)
		{
			if(node.Children != null)
			{
				// This node is a branch, so recurse on its children.
				foreach(QuadtreeNode child in node.Children)
				{
					DrawTerrainQuadtreeSub(child, basicEffect, depth + 1);
				}
			}

			// Draw the node's own bounding box.
			var colours = new Color[] { Color.Cyan, Color.Yellow, Color.Magenta };
			Renderer.DrawBoundingBox(node.Bounds, basicEffect, colours[depth % colours.Length]);
		}

		#endregion
	}
}
