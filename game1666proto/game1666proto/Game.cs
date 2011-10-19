/***
 * game1666proto: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto
{
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private BasicEffect m_basicEffect;
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
			m_basicEffect.View = Matrix.CreateLookAt(new Vector3(15, -30, 20), new Vector3(0, 0, 0), new Vector3(0, 0, 1));

			// Set up the world matrix.
			m_basicEffect.World = Matrix.Identity;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Draw the city.
			m_city.Draw(GraphicsDevice, ref m_basicEffect, m_landscapeTexture);

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
			m_city = new City();
			m_city.AddBuilding(new Building(new Vector2(2,3)));
			m_city.AddBuilding(new Building(new Vector2(-1,-2)));

			// Register input handlers.
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
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse press check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			Viewport viewport = GraphicsDevice.Viewport;

			// Find the point we're clicking on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the point we're clicking on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Find where (if anywhere) the ray intersects the city plane.
			float? distance = ray.Intersects(new Plane(new Vector3(0, 0, 1), 0));
			if(distance != null)
			{
				// Determine the designated position.
				Vector3 pos = near + dir * distance.Value;
				
				// Create a new building at the designated position.
				m_city.AddBuilding(new Building(new Vector2(pos.X, pos.Y)));
			}
		}

		#endregion
	}
}
