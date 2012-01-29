/***
 * game1666proto4: OccupancyMap.cs
 * Copyright 2011. All rights reserved.
 ***/

using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class stores occupancy information for a terrain.
	/// </summary>
	sealed class OccupancyMap
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain for which to store occupancy information.
		/// </summary>
		public Terrain Terrain { private get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a road segment to the occupancy map.
		/// </summary>
		/// <param name="roadSegment">The road segment.</param>
		public void AddEntity(RoadSegment roadSegment)
		{
			// TODO
		}

		#endregion
	}
}
