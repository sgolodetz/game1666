/***
 * game1666proto2: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto2
{
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private BasicEffect m_basicEffect;
		private Building m_buildingToPlace;
		private City m_city;
		private readonly GraphicsDeviceManager m_graphics;
		private Texture2D m_landscapeTexture;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and window, and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphics = new GraphicsDeviceManager(this);
			if(!GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphics.GraphicsProfile = GraphicsProfile.Reach;
			}
			m_graphics.PreferredBackBufferWidth = 640;
			m_graphics.PreferredBackBufferHeight = 480;

			this.IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Set up the view matrix.
			m_basicEffect.View = Matrix.CreateLookAt(new Vector3(5, -20, 20), new Vector3(5, 20, 0), new Vector3(0, 0, 1));

			// Set up the world matrix.
			m_basicEffect.World = Matrix.Identity;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Draw the city.
			m_city.Draw(GraphicsDevice, ref m_basicEffect, m_landscapeTexture);

			// Draw the building to be placed (if any).
			if(m_buildingToPlace != null)
			{
				m_buildingToPlace.Draw(GraphicsDevice, ref m_basicEffect);
			}

			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Set up the basic effect.
			m_basicEffect = new BasicEffect(GraphicsDevice);
			
			// Set up the projection matrix.
			m_basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)m_graphics.PreferredBackBufferWidth / m_graphics.PreferredBackBufferHeight, 0.1f, 1000.0f);

			// Set up the city.
			var terrain = new int[][]
			{
				new int[] {1,2,2,1},
				new int[] {1,1,1,1},
				new int[] {1,1,1,1},
				new int[] {2,1,1,2},
				new int[] {3,2,2,3},
				new int[] {4,2,2,4},
				new int[] {4,2,2,4},
				new int[] {4,2,2,4}
			};
			m_city = new City(terrain);

			// Register input handlers.
			MouseEventManager.OnMouseMoved += OnMouseMoved;
			MouseEventManager.OnMousePressed += OnMousePressed;

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			m_landscapeTexture = Content.Load<Texture2D>("landscape");
		}

		/// <summary>
		/// Called once per game to allow content to be unloaded.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			MouseEventManager.Update();

			base.Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMouseMoved(MouseState state)
		{
			m_buildingToPlace = null;

			Viewport viewport = GraphicsDevice.Viewport;

			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Find the terrain triangle we're hovering over (if any).
			PickedTriangle? pickedTriangle = m_city.PickTerrainTriangle(ray);

			// If we're hovering over a terrain triangle, set up a temporary building there to show where we would build.
			if(pickedTriangle != null)
			{
				m_buildingToPlace = new Building(pickedTriangle.Value.Triangle);
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			/*Viewport viewport = GraphicsDevice.Viewport;

			// Find the point we're clicking on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the point we're clicking on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Find the terrain triangle we're trying to click (if any).
			PickedTriangle? pickedTriangle = m_city.PickTerrainTriangle(ray);
			// TODO*/
		}

		#endregion
	}
}
