/***
 * game1666proto4: IEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity in the game.
	/// </summary>
	interface IEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the entity (if any).
		/// </summary>
		string Name { get; }

		#endregion
	}
}
