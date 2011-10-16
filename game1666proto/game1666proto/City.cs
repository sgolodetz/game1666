/***
 * game1666: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto
{
	/// <summary>
	/// Represents a city.
	/// </summary>
	class City
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private List<Building> m_buildings = new List<Building>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The new building to be added.</param>
		public void AddBuilding(Building building)
		{
			m_buildings.Add(building);
		}

		/// <summary>
		/// Draws the city.
		/// </summary>
		public void Draw()
		{
			// TODO
		}

		#endregion
	}
}
