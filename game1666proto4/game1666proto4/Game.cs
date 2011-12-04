/***
 * game1666proto4: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents the game itself.
	/// </summary>
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly GraphicsDeviceManager m_graphicsDeviceManager;
		private ViewManager m_viewManager;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and window, and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphicsDeviceManager = new GraphicsDeviceManager(this);
			if(GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
			}
			m_graphicsDeviceManager.PreferredBackBufferWidth = 800;
			m_graphicsDeviceManager.PreferredBackBufferHeight = 600;
			m_graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

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

			m_viewManager.Draw();

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
			// Set up the renderer.
			Renderer.Content = Content;
			Renderer.GraphicsDevice = GraphicsDevice;

			// Load the world from an XML file.
			SceneGraph.LoadWorld(@"Content\TestWorld.xml");

			// Look up the view manager in the scene graph.
			m_viewManager = SceneGraph.GetEntityByPath("views");

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Pre-load content.
			Content.Load<Texture2D>("landscape");
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

			MouseEventManager.Update();
			//m_playingAreaViewer.Update(gameTime);

			base.Update(gameTime);
		}

		#endregion
	}
}
