/***
 * game1666proto4: IMovementStrategy.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Navigation;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a movement strategy for a mobile entity (e.g. "Go To Position").
	/// </summary>
	interface IMovementStrategy : IPersistableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		IDictionary<string,dynamic> EntityProperties { set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		EntityNavigationMap NavigationMap { set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tries to move the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>true, if the entity was able to move, or false otherwise.</returns>
		bool Move(GameTime gameTime);

		#endregion
	}
}
