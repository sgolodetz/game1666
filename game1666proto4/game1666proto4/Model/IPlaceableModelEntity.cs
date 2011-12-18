/***
 * game1666proto4: IPlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be placed in a playing area.
	/// </summary>
	interface IPlaceableModelEntity
	{
		//#################### PROPERTIES ####################

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		Blueprint Blueprint { get; }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		Vector2i Position { get; }
	}
}
