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
	sealed class Building : IPlaceableModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Footprint m_footprint;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public Footprint Footprint { get { return m_footprint; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Building(Footprint footprint)
		{
			m_footprint = footprint;
		}

		#endregion
	}
}
