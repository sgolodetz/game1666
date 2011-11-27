/***
 * game1666proto4: PlayingAreaSidebar.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class PlayingAreaSidebar
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly PlayingArea m_playingArea;
		private readonly Viewport m_viewport;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar for the specified playing area.
		/// </summary>
		/// <param name="playingArea">The playing area.</param>
		/// <param name="viewport">The viewport into which to draw the sidebar.</param>
		public PlayingAreaSidebar(PlayingArea playingArea, Viewport viewport)
		{
			m_playingArea = playingArea;
			m_viewport = viewport;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the sidebar for the playing area.
		/// </summary>
		public void Draw()
		{
			Renderer.GraphicsDevice.Viewport = m_viewport;
			Renderer.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = false };
			Texture2D sprite = Renderer.Content.Load<Texture2D>("landscape");
			var spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			spriteBatch.Begin();
			spriteBatch.Draw(sprite, m_viewport.Bounds, Color.White);
			spriteBatch.End();
			Renderer.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = false };
		}

		#endregion
	}
}
