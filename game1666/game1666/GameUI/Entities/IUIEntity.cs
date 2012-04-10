﻿/***
 * game1666: UIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component-based entity that is part of the game's UI.
	/// </summary>
	interface IUIEntity : IEntity<IUIEntity>
	{
		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		Viewport Viewport { get; }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		IBasicEntity World { get; }
	}
}