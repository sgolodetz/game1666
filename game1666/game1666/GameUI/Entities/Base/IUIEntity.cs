/***
 * game1666: IUIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Base
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component-based entity that is part of the game's UI.
	/// </summary>
	interface IUIEntity : IEntity<IUIEntity>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		Viewport Viewport { get; }

		#endregion
	}
}
