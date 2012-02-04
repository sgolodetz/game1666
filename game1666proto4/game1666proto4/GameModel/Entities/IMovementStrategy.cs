/***
 * game1666proto4: IMovementStrategy.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy for a mobile entity (e.g. "Go To Position").
	/// </summary>
	interface IMovementStrategy
	{
		/// <summary>
		/// Moves the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Move(GameTime gameTime);
	}
}
