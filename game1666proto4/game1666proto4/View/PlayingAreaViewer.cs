/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class can be used to view a playing area.
	/// </summary>
	sealed class PlayingAreaViewer
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

		/// <summary>
		/// The viewport into which to draw the playing area.
		/// </summary>
		private readonly Viewport m_viewport;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a viewer for the specified playing area.
		/// </summary>
		/// <param name="playingArea">The playing area to view.</param>
		/// <param name="viewport">The viewport into which to draw the playing area.</param>
		public PlayingAreaViewer(PlayingArea playingArea, Viewport viewport)
		{
			m_playingArea = playingArea;
			m_viewport = viewport;
			m_camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), new Vector3(0,0,1));

			// Register input handlers.
			MouseEventManager.OnMousePressed += OnMousePressed;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the playing area.
		/// </summary>
		public void Draw()
		{
			BasicEffect viewerBasicEffect = CreateViewerBasicEffect();

			DrawTerrain(viewerBasicEffect);
#if DEBUG
			DrawTerrainQuadtree(viewerBasicEffect);
#endif
		}

		/// <summary>
		/// Updates the viewer from one frame to the next, taking user input into account.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// Determine the linear, horizontal angular, and vertical angular movement rates.
			float linearRate = 0.006f * gameTime.ElapsedGameTime.Milliseconds;
			float angularRateH = 0.002f * gameTime.ElapsedGameTime.Milliseconds;	// in radians
			float angularRateV = 0.0015f * gameTime.ElapsedGameTime.Milliseconds;	// in radians

			// Alter the camera based on user input.
			KeyboardState state = Keyboard.GetState();
			if(state.IsKeyDown(Keys.W))		m_camera.MoveN(linearRate);
			if(state.IsKeyDown(Keys.S))		m_camera.MoveN(-linearRate);
			if(state.IsKeyDown(Keys.A))		m_camera.MoveU(linearRate);
			if(state.IsKeyDown(Keys.D))		m_camera.MoveU(-linearRate);
			if(state.IsKeyDown(Keys.Q))		m_camera.MoveV(linearRate);
			if(state.IsKeyDown(Keys.E))		m_camera.MoveV(-linearRate);
			if(state.IsKeyDown(Keys.Left))	m_camera.Rotate(new Vector3(0,0,1), angularRateH);
			if(state.IsKeyDown(Keys.Right))	m_camera.Rotate(new Vector3(0,0,1), -angularRateH);
			if(state.IsKeyDown(Keys.Up))	m_camera.Rotate(m_camera.U, angularRateV);
			if(state.IsKeyDown(Keys.Down))	m_camera.Rotate(m_camera.U, -angularRateV);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Creates the underlying basic effect for the viewer as a whole (ready to be customised).
		/// </summary>
		/// <returns>The basic effect.</returns>
		private BasicEffect CreateViewerBasicEffect()
		{
			BasicEffect viewerBasicEffect = Renderer.DefaultBasicEffect.Clone() as BasicEffect;

			// Set up the view matrix.
			viewerBasicEffect.View = Matrix.CreateLookAt(m_camera.Position, m_camera.Position + m_camera.N, m_camera.V);

			// Set up the world matrix.
			viewerBasicEffect.World = Matrix.Identity;

			return viewerBasicEffect;
		}

		/// <summary>
		/// Draws the playing area's terrain.
		/// </summary>
		/// <param name="viewerBasicEffect">The underlying basic effect for the viewer as a whole (ready to be customised).</param>
		private void DrawTerrain(BasicEffect viewerBasicEffect)
		{
			BasicEffect basicEffect = viewerBasicEffect.Clone() as BasicEffect;
			basicEffect.Texture = Renderer.Content.Load<Texture2D>("landscape");
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

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			BasicEffect viewerBasicEffect = CreateViewerBasicEffect();

			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = m_viewport.Unproject(new Vector3(state.X, state.Y, 0), viewerBasicEffect.Projection, viewerBasicEffect.View, viewerBasicEffect.World);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = m_viewport.Unproject(new Vector3(state.X, state.Y, 1), viewerBasicEffect.Projection, viewerBasicEffect.View, viewerBasicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Output the grid square clicked by the user.
			Vector2i? gridSquare = m_playingArea.Terrain.PickGridSquare(ray);
			if(gridSquare != null)	System.Console.WriteLine(gridSquare.Value.X.ToString() + ' ' + gridSquare.Value.Y.ToString());
			else					System.Console.WriteLine("No grid square was clicked");
		}

		#endregion
	}
}
