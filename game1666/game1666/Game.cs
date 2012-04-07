/***
 * game1666: Game.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.Persistence;
using game1666.Common.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666
{
	/// <summary>
	/// An instance of this class represents the game itself.
	/// </summary>
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly GraphicsDeviceManager m_graphicsDeviceManager;
		/*private ViewHierarchy m_viewHierarchy;
		private World m_world;*/

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
			m_graphicsDeviceManager.PreferredBackBufferWidth = 1024;
			m_graphicsDeviceManager.PreferredBackBufferHeight = 768;
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

			//m_viewHierarchy.Draw();

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

			// Register special XML elements with the object persister.
			ObjectPersister.RegisterSpecialElement("entity", typeof(Entity));

			// Load the world from an XML file.
			/*m_world = World.LoadFromFile(@"Content\PathfindingWorld.xml");

			// Load the view hierarchy from the game configuration file.
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			m_viewHierarchy = new ViewHierarchy(doc.XPathSelectElement("config/views"), m_world);*/

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Pre-load content.
			Content.Load<Texture2D>("Textures/grass");
			Content.Load<Texture2D>("Textures/snow");
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
			KeyboardState keyState = Keyboard.GetState();
			if(keyState.IsKeyDown(Keys.Escape))	Exit();
			/*if(keyState.IsKeyDown(Keys.F1))		m_viewHierarchy.CurrentView = "City";
			if(keyState.IsKeyDown(Keys.F2))		m_viewHierarchy.CurrentView = "World";*/

			MouseEventManager.Update();
			/*m_viewHierarchy.Update(gameTime);
			m_world.Update(gameTime);*/

			base.Update(gameTime);
		}

		#endregion
	}
}
