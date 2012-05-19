/***
 * game1666: ButtonRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.UI;
using game1666.GameUI.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this class provides rendering behaviour to a button.
	/// </summary>
	sealed class ButtonRenderingComponent : RenderingComponent, IButtonRenderingComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A hook that can be used to check whether or not the button should be drawn highlighted.
		/// </summary>
		private Func<bool> m_isHighlighted = () => false;

		/// <summary>
		/// The sprite batch to use when drawing the button.
		/// </summary>
		private readonly SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture to use when drawing the button.
		/// </summary>
		private readonly Texture2D m_texture;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A hook that can be used to check whether or not the button should be drawn highlighted.
		/// </summary>
		public Func<bool> IsHighlighted
		{
			private get	{ return m_isHighlighted; }
			set			{ m_isHighlighted = value; }
		}

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "ButtonRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a button rendering component.
		/// </summary>
		/// <param name="textureName">The name of the texture to use when drawing the button.</param>
		public ButtonRenderingComponent(string textureName)
		{
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("Textures/" + textureName);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the button of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Entity.Viewport;

			// Draw the button texture.
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(m_texture, new Rectangle(0, 0, Entity.Viewport.Width, Entity.Viewport.Height), Color.White);
			m_spriteBatch.End();

			// Draw a surrounding rectangle if the button is highlighted.
			if(IsHighlighted())
			{
				BasicEffect effect = Renderer.Line2DEffect(Entity.Viewport);
				Renderer.DrawBoundingBox(new Vector2(0, 0), new Vector2(Entity.Viewport.Width - 1, Entity.Viewport.Height - 1), effect, Color.Red);
			}
		}

		#endregion
	}
}
