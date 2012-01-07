﻿/***
 * game1666proto4: IUserInterfaceEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.Common.Entities2
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that will form part of the game's user interface.
	/// </summary>
	interface IUserInterfaceEntity : IUpdateableEntity
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
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		void OnMouseMoved(MouseState state);

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		void OnMousePressed(MouseState state);

		#endregion
	}
}
