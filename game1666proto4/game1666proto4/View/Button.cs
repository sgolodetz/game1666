/***
 * game1666proto4: Button.cs
 * Copyright 2011. All rights reserved.
 ***/

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
		/// The name of the texture to use when drawing the button.
		/// </summary>
		private string m_textureName;

		/// <summary>
		/// The viewport specifying the area taken up by the button on the screen.
		/// </summary>
		private Viewport m_viewport;

		#endregion

		//#################### EVENTS ####################
		#region

		/// <summary>
		/// Invoked when a mouse button is pressed.
		/// </summary>
		public static event MouseEvent MousePressedHook = delegate {};

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
			m_textureName = textureName;
			m_viewport = viewport;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the button.
		/// </summary>
		public override void Draw()
		{
			// TODO
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
