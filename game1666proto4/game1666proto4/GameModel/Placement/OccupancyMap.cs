/***
 * game1666proto4: OccupancyMap.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Entities;

namespace game1666proto4.GameModel.Placement
{
	/// <summary>
	/// An instance of this class stores occupancy information for a terrain.
	/// </summary>
	sealed class OccupancyMap
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// An occupancy grid indicating which grid squares are currently occupied, e.g. by buildings.
		/// </summary>
		private bool[,] m_occupancy;

		/// <summary>
		/// The terrain for which to store occupancy information.
		/// </summary>
		private Terrain m_terrain;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain for which to store occupancy information.
		/// </summary>
		public Terrain Terrain
		{
			get
			{
				return m_terrain;
			}

			set
			{
				m_terrain = value;
				m_occupancy = new bool[m_terrain.Heightmap.GetLength(0) - 1, m_terrain.Heightmap.GetLength(1) - 1];
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not any of the specified grid squares are occupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to check.</param>
		/// <returns>true, if any of the grid squares are occupied, or false otherwise.</returns>
		public bool AreOccupied(IEnumerable<Vector2i> gridSquares)
		{
			return gridSquares.Any(s => m_occupancy[s.Y, s.X]);
		}

		/// <summary>
		/// Marks a set of grid squares as occupied/unoccupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to mark.</param>
		/// <param name="occupied">Whether or not the grid squares are now occupied.</param>
		public void MarkOccupied(IEnumerable<Vector2i> gridSquares, bool occupied)
		{
			foreach(Vector2i s in gridSquares)
			{
				m_occupancy[s.Y, s.X] = occupied;
			}
		}

		#endregion
	}
}
