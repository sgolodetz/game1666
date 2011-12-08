/***
 * game1666proto4: Button.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a clickable button in one of the views.
	/// </summary>
	sealed class Button : ViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The sprite batch used when drawing the button.
		/// </summary>
		private readonly SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture used when drawing the button.
		/// </summary>
		private readonly Texture2D m_texture;

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
		{
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>(textureName);
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
			m_spriteBatch.Begin();
			m_spriteBatch.Draw(m_texture, new Rectangle(0, 0, Viewport.Width, Viewport.Height), Color.White);
			m_spriteBatch.End();
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			MousePressedHook(state);
		}

		#endregion
	}
}
