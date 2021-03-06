﻿/***
 * game1666: SidebarRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this class draws a sidebar that allows the user to interact with a playing area (such as the world or a settlement).
	/// </summary>
	sealed class SidebarRenderingComponent : CompositeRenderingComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The sprite batch used when drawing the sidebar.
		/// </summary>
		private SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture used when drawing the sidebar.
		/// </summary>
		private Texture2D m_texture;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "SidebarRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public SidebarRenderingComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("Textures/sidebarbackground");
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the sidebar viewer of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Entity.Viewport;
			Renderer.Setup2D();

			// Draw the sidebar background.
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(m_texture, new Rectangle(0, 0, Entity.Viewport.Width, Entity.Viewport.Height), Color.White);
			m_spriteBatch.End();

			// Draw the sidebar buttons.
			base.Draw();
		}

		#endregion
	}
}
