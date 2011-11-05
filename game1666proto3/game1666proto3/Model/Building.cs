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
	sealed class Building : IPlaceableModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly EntityFootprint m_footprint;
		private readonly EntityOrientation m_orientation;
		private readonly Tuple<int,int> m_position;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public EntityFootprint Footprint { get { return m_footprint; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Building(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation)
		{
			m_footprint = footprint;
			m_position = position;
			m_orientation = orientation;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public bool ValidateFootprint(TerrainMesh terrainMesh)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
