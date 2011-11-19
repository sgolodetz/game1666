/***
 * game1666proto4: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private BasicEffect m_basicEffect;
		private readonly GraphicsDeviceManager m_graphics;
		private PlayingAreaViewer m_viewer;
		private World m_world;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and window, and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphics = new GraphicsDeviceManager(this);
			if(GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
			m_basicEffect.View = Matrix.CreateLookAt(new Vector3(10, -20, 20), new Vector3(5, 20, 0), new Vector3(0, 0, 1));

			// Set up the world matrix.
			m_basicEffect.World = Matrix.Identity;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Draw the playing area.
			m_viewer.Draw();

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
			// Load the game configuration from XML.
			GameConfig.Load(@"Content\GameConfig.xml");

			// Set up the basic effect.
			m_basicEffect = new BasicEffect(GraphicsDevice);
			
			// Set up the projection matrix.
			m_basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)m_graphics.PreferredBackBufferWidth / m_graphics.PreferredBackBufferHeight, 0.1f, 1000.0f);

			// Set up the world.
			m_world = new World(new Terrain(new float[2,2], 5f, 5f));
			var city = new City("Stuartopolis", new Terrain(new float[2,2], 5f, 5f));
			m_world.AddEntity(city);

			// Set up the viewer.
			m_viewer = new PlayingAreaViewer(city);

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Pre-load content.
			// TODO
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
			if(Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			// TODO

			base.Update(gameTime);
		}

		#endregion
	}
}
