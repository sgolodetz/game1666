/***
 * game1666proto3: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	enum BuildingOrientation
	{
		RIGHT,
		UP,
		LEFT,
		DOWN
	}

	/// <summary>
	/// Represents a building in the game model.
	/// </summary>
	sealed class Building : IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly BuildingFootprint m_footprint;
		private readonly BuildingOrientation m_orientation;
		private readonly Tuple<int,int> m_position;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Building(BuildingFootprint footprint, Tuple<int,int> position, BuildingOrientation orientation = BuildingOrientation.RIGHT)
		{
			m_footprint = footprint;
			m_orientation = orientation;
			m_position = position;
		}

		#endregion
	}
}
