/***
 * game1666proto3: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	/// <summary>
	/// Represents a building in the game model.
	/// </summary>
	sealed class Building : PlaceableModelEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified footprint, position and orientation.
		/// </summary>
		/// <param name="footprint">The footprint of the building.</param>
		/// <param name="position">The position of the building's hotspot.</param>
		/// <param name="orientation">The orientation of the building.</param>
		public Building(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation)
		:	base(footprint, position, orientation)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the building could be validly placed on the terrain mesh, given its position and orientation.
		/// </summary>
		/// <param name="terrainMesh">The terrain mesh against which to validate the building.</param>
		/// <returns>true, if the building could be validly placed, or false otherwise</returns>
		public override bool ValidateFootprint(TerrainMesh terrainMesh)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
