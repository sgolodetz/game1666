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

		public Building(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation)
		:	base(footprint, position, orientation)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public override bool ValidateFootprint(TerrainMesh terrainMesh)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
