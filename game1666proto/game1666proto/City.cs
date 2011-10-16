/***
 * game1666: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto
{
	class City
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private List<Building> m_buildings = new List<Building>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public void AddBuilding(Building building)
		{
			m_buildings.Add(building);
		}

		#endregion
	}
}
