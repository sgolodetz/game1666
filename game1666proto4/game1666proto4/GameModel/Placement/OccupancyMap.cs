/***
 * game1666proto4: OccupancyMap.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;

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
		/// An occupancy grid indicating the current occupancy of each grid square (e.g. a square might be occupied by a building).
		/// </summary>
		private IPlaceableEntity[,] m_occupancy;

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
				m_occupancy = new IPlaceableEntity[m_terrain.Heightmap.GetLength(0) - 1, m_terrain.Heightmap.GetLength(1) - 1];
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
			return gridSquares.Any(s => m_occupancy[s.Y, s.X] != null);
		}

		/// <summary>
		/// Looks up the entity (if any) that occupies the specified grid square.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <returns>The entity occupying it, if any, or null otherwise.</returns>
		public IPlaceableEntity LookupEntity(Vector2i gridSquare)
		{
			if(0 <= gridSquare.Y && gridSquare.Y < m_occupancy.GetLength(0) &&
			   0 <= gridSquare.X && gridSquare.X < m_occupancy.GetLength(1))
			{
				return m_occupancy[gridSquare.Y, gridSquare.X];
			}
			else return null;
		}

		/// <summary>
		/// Checks whether or not an entity can be validly placed on the terrain,
		/// bearing in mind its footprint, position and orientation.
		/// </summary>
		/// <returns>true, if the entity can be validly placed, or false otherwise.</returns>
		public bool IsValidlyPlaced(IPlaceableEntity entity)
		{
			IEnumerable<Vector2i> gridSquares = entity.PlacementStrategy.Place
			(
				Terrain,
				entity.Blueprint.Footprint,
				entity.Position,
				entity.Orientation
			);
			return gridSquares != null && gridSquares.Any() && !AreOccupied(gridSquares);
		}

		/// <summary>
		/// Marks a set of grid squares with the entity they contain (if any).
		/// </summary>
		/// <param name="gridSquares">The grid squares to mark.</param>
		/// <param name="entity">The entity they contain (if any).</param>
		public void MarkOccupied(IEnumerable<Vector2i> gridSquares, IPlaceableEntity entity)
		{
			foreach(Vector2i s in gridSquares)
			{
				m_occupancy[s.Y, s.X] = entity;
			}
		}

		#endregion
	}
}
