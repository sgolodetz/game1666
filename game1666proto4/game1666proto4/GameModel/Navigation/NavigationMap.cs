/***
 * game1666proto4: NavigationMap.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of this class handles navigation for a terrain.
	/// </summary>
	sealed class NavigationMap<PlaceableEntityType> where PlaceableEntityType : class
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// An occupancy grid indicating the current occupancy of each grid square (e.g. a square might be occupied by a building).
		/// </summary>
		private PlaceableEntityType[,] m_occupancy;

		/// <summary>
		/// The terrain for which to handle navigation.
		/// </summary>
		private Terrain m_terrain;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain for which to handle navigation.
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
				m_occupancy = new PlaceableEntityType[m_terrain.Heightmap.GetLength(0) - 1, m_terrain.Heightmap.GetLength(1) - 1];
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
		/// Finds a path from the specified source to the nearest of the specified destinations.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destinations">The destinations.</param>
		/// <returns>The path, as a list of points to traverse, or null if no path can be found.</returns>
		public List<Vector2> FindPath(Vector2 source, List<Vector2> destinations)
		{
			var sourceSquare = source.Discretize();
			var destinationSquares = destinations.Select(v => v.Discretize()).ToList();

			// TODO
			return null;
		}

		/// <summary>
		/// Looks up the entity (if any) that occupies the specified grid square.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <returns>The entity occupying it, if any, or null otherwise.</returns>
		public PlaceableEntityType LookupEntity(Vector2i gridSquare)
		{
			if(0 <= gridSquare.Y && gridSquare.Y < m_occupancy.GetLength(0) &&
			   0 <= gridSquare.X && gridSquare.X < m_occupancy.GetLength(1))
			{
				return m_occupancy[gridSquare.Y, gridSquare.X];
			}
			else return null;
		}

		/// <summary>
		/// Marks a set of grid squares with the entity they contain (if any).
		/// </summary>
		/// <param name="gridSquares">The grid squares to mark.</param>
		/// <param name="entity">The entity they contain (if any).</param>
		public void MarkOccupied(IEnumerable<Vector2i> gridSquares, PlaceableEntityType entity)
		{
			if(gridSquares == null) return;

			foreach(Vector2i s in gridSquares)
			{
				m_occupancy[s.Y, s.X] = entity;
			}
		}

		#endregion
	}
}
