/***
 * game1666proto4: NavigationMap.cs
 * Copyright 2012. All rights reserved.
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
	/// <typeparam name="PlaceableEntityType">The type of entity that gets placed on the terrain.</typeparam>
	/// <typeparam name="NavigationNodeType">The type of navigation node to be used.</typeparam>
	class NavigationMap<PlaceableEntityType,NavigationNodeType>
		where PlaceableEntityType : class
		where NavigationNodeType : INavigationNode<PlaceableEntityType,NavigationNodeType>, new()
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The grid of navigation nodes, used for pathfinding and occupancy checking.
		/// </summary>
		private NavigationNodeType[,] m_nodeGrid;

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

				int gridHeight = m_terrain.Heightmap.GetLength(0) - 1;
				int gridWidth = m_terrain.Heightmap.GetLength(1) - 1;
				m_nodeGrid = new NavigationNodeType[gridHeight, gridWidth];
				for(int y = 0; y < gridHeight; ++y)
				{
					for(int x = 0; x < gridWidth; ++x)
					{
						m_nodeGrid[y,x] = new NavigationNodeType().Initialise(new Vector2i(x,y), m_nodeGrid, m_terrain);
					}
				}
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
			return gridSquares.Any(s => m_nodeGrid[s.Y, s.X].OccupyingEntity != null);
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
			if(0 <= gridSquare.Y && gridSquare.Y < m_nodeGrid.GetLength(0) &&
			   0 <= gridSquare.X && gridSquare.X < m_nodeGrid.GetLength(1))
			{
				return m_nodeGrid[gridSquare.Y, gridSquare.X].OccupyingEntity;
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
				m_nodeGrid[s.Y, s.X].OccupyingEntity = entity;
			}
		}

		#endregion
	}
}
