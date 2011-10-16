/***
 * game1666: Game.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private City m_city;
		private GraphicsDeviceManager m_graphics;
		private SpriteBatch m_spriteBatch;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphics = new GraphicsDeviceManager(this);
			if(!GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphics.GraphicsProfile = GraphicsProfile.Reach;
			}

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

			m_city.Draw();

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
			m_city = new City();
			m_city.AddBuilding(new Building(new Vector2(2,3)));
			m_city.AddBuilding(new Building(new Vector2(-1,-2)));

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			m_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// Called once per game to allow content to be unloaded.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		#endregion
	}
}
