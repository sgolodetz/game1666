/***
 * game1666proto4: SidebarViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class SidebarViewer : Entity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private PlayingArea m_playingArea;
		private SpriteBatch m_spriteBatch;
		private Viewport m_viewport;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar viewer for the specified playing area.
		/// </summary>
		/// <param name="playingArea">The playing area.</param>
		/// <param name="viewport">The viewport into which to draw the sidebar.</param>
		public SidebarViewer(PlayingArea playingArea, Viewport viewport)
		{
			m_playingArea = playingArea;
			m_viewport = viewport;
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
		}

		public SidebarViewer(string playingAreaSpecifier, string viewportSpecifier)
		{
			Properties["PlayingArea"] = playingAreaSpecifier;
			Properties["Viewport"] = viewportSpecifier;
			Initialise();
		}

		public SidebarViewer(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		private void Initialise()
		{
			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_viewport = ViewUtil.ParseViewportSpecifier(Properties["Viewport"]);
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
			Texture2D sprite = Renderer.Content.Load<Texture2D>("landscape");
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(sprite, new Rectangle(0, 0, m_viewport.Width, m_viewport.Height), Color.White);
			m_spriteBatch.End();
		}

		#endregion
	}
}
