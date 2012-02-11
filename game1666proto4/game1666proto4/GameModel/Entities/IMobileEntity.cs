/***
 * game1666proto4: IMobileEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Navigation;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can move (e.g. a walker).
	/// </summary>
	interface IMobileEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		MobileEntityBlueprint Blueprint { get; set; }

		/// <summary>
		/// The movement strategy for the entity.
		/// </summary>
		IMovementStrategy MovementStrategy { set; }

		/// <summary>
		/// The name of the entity (must be unique within its playing area).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The occupancy map for the terrain on which the entity is moving.
		/// </summary>
		OccupancyMap<IPlaceableEntity> OccupancyMap { set; }

		/// <summary>
		/// The 2D 45-degree orientation of the entity.
		/// </summary>
		Orientation8 Orientation { get; }

		/// <summary>
		/// The position of the entity (relative to the origin of the containing entity).
		/// </summary>
		Vector3 Position { get; }

		#endregion
	}
}
