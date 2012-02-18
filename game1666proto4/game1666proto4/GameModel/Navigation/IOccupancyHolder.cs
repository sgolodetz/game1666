/***
 * game1666proto4: IOccupancyHolder.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of a class implementing this interface holds occupancy information
	/// indicating the entity (if any) occupying a particular grid square on the terrain.
	/// </summary>
	/// <typeparam name="PlaceableEntityType">The type of entity that can be placed on the terrain.</typeparam>
	interface IOccupancyHolder<PlaceableEntityType>
	{
		/// <summary>
		/// The entity occupying the grid square (if any), or null otherwise.
		/// </summary>
		PlaceableEntityType OccupyingEntity { get; set; }
	}
}
