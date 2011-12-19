/***
 * game1666proto4: IUpdateableEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be updated over time.
	/// </summary>
	interface IUpdateableEntity : IEntity
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the entity based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
