/***
 * game1666proto4: IVisibleEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that will be drawn on the screen.
	/// </summary>
	interface IVisibleEntity : IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		Viewport Viewport { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity.
		/// </summary>
		void Draw();

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		void OnMousePressed(MouseState state);

		#endregion
	}
}
