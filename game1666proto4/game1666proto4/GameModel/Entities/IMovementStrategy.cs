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
	/// This enum specifies the possible results of trying to move an entity.
	/// </summary>
	enum MoveResult
	{
		BLOCKED,	// the entity is currently blocked, but will try and move again next time
		FINISHED,	// the entity has finished whatever it was trying to do and needs a new goal
		MOVED		// the entity successfully moved
	}

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
		/// <returns>The result of the attempt: either blocked, finished or moved.</returns>
		MoveResult Move(GameTime gameTime);

		#endregion
	}
}
