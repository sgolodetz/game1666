/***
 * game1666proto4: Button.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Graphics;
using game1666proto4.Common.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class represents a clickable button in one of the views.
	/// </summary>
	sealed class Button : VisibleEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A hook that the button can use to check whether or not it should be drawn highlighted.
		/// </summary>
		private Func<bool> m_isHighlighted = () => false;

		/// <summary>
		/// The sprite batch used when drawing the button.
		/// </summary>
		private readonly SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture used when drawing the button.
		/// </summary>
		private readonly Texture2D m_texture;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A hook that the button can use to check whether or not it should be drawn highlighted.
		/// </summary>
		public Func<bool> IsHighlighted { private get { return m_isHighlighted; } set { m_isHighlighted = value; } }

		#endregion

		//#################### EVENTS ####################
		#region

		/// <summary>
		/// Invoked when a mouse button is pressed.
		/// </summary>
		public event MouseEvent MousePressedHook = delegate {};

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a button.
		/// </summary>
		/// <param name="textureName">The name of the texture to use when drawing the button.</param>
		/// <param name="viewport">The viewport specifying the area taken up by the button on the screen.</param>
		public Button(string textureName, Viewport viewport)
		:	base(null)
		{
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("Textures/" + textureName);
			Viewport = viewport;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the button.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Viewport;

			// Draw the button texture.
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(m_texture, new Rectangle(0, 0, Viewport.Width, Viewport.Height), Color.White);
			m_spriteBatch.End();

			// Draw a surrounding rectangle if the button is highlighted.
			if(IsHighlighted())
			{
				BasicEffect effect = Renderer.Line2DEffect(Viewport);
				Renderer.DrawBoundingBox(new Vector2(0, 0), new Vector2(Viewport.Width - 1, Viewport.Height - 1), effect, Color.Red);
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			MousePressedHook(state);
		}

		#endregion
	}
}
