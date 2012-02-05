/***
 * game1666proto4: IMovementStrategy.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a movement strategy for a mobile entity (e.g. "Go To Position").
	/// </summary>
	interface IMovementStrategy
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		IDictionary<string,dynamic> EntityProperties { set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Moves the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Move(GameTime gameTime);

		#endregion
	}
}
