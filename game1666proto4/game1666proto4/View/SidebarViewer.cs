/***
 * game1666proto4: SidebarViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class SidebarViewer : ViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The playing area whose entities this sidebar is used to manipulate.
		/// </summary>
		private PlayingArea m_playingArea;

		/// <summary>
		/// The sprite batch used when drawing the sidebar.
		/// </summary>
		private SpriteBatch m_spriteBatch;

		/// <summary>
		/// The viewport into which to draw the sidebar.
		/// </summary>
		private Viewport m_viewport;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar viewer for the specified playing area.
		/// </summary>
		/// <param name="playingAreaSpecifier">The entity path of the specified playing area.</param>
		/// <param name="viewportSpecifier">A string specifying the viewport into which to draw the sidebar.</param>
		public SidebarViewer(string playingAreaSpecifier, string viewportSpecifier)
		{
			Properties["PlayingArea"] = playingAreaSpecifier;
			Properties["Viewport"] = viewportSpecifier;
			Initialise();
		}

		/// <summary>
		/// Constructs a sidebar viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		public SidebarViewer(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the sidebar for the playing area.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = m_viewport;
			Texture2D sprite = Renderer.Content.Load<Texture2D>("landscape");
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(sprite, new Rectangle(0, 0, m_viewport.Width, m_viewport.Height), Color.White);
			m_spriteBatch.End();
		}

		/// <summary>
		/// Updates the viewer based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// No-op
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the viewer based on its properties.
		/// </summary>
		private void Initialise()
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_viewport = ViewUtil.ParseViewportSpecifier(Properties["Viewport"]);
		}

		#endregion
	}
}
