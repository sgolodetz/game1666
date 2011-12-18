/***
 * game1666proto4: IBuildableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be built in a playing area.
	/// </summary>
	interface IBuildableModelEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		float Altitude { get; }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		Blueprint Blueprint { get; }

		/// <summary>
		/// The builder for the entity.
		/// </summary>
		EntityBuilder Builder { get; }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		Vector2i Position { get; }

		#endregion
	}
}
