/***
 * game1666proto4: IUpdateableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4.Common.Entities2
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be updated over time.
	/// </summary>
	interface IUpdateableEntity
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
