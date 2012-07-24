/***
 * game1666: NavigationMap.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666.Common.AStar;
using game1666.Common.Maths;
using game1666.GameModel.Terrains;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Navigation
{
	/// <summary>
	/// An instance of this class handles navigation for a terrain.
	/// </summary>
	/// <typeparam name="PlaceableEntityType">The type of entity that gets placed on the terrain.</typeparam>
	/// <typeparam name="NavigationNodeType">The type of navigation node to be used.</typeparam>
	sealed class NavigationMap<PlaceableEntityType,NavigationNodeType> : INavigationMap<PlaceableEntityType>
		where PlaceableEntityType : class
		where NavigationNodeType : AStarNode<NavigationNodeType>, INavigationNode<PlaceableEntityType,NavigationNodeType>, new()
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The grid of navigation nodes, used for pathfinding and occupancy checking.
		/// </summary>
		private readonly NavigationNodeType[,] m_nodeGrid;

		/// <summary>
		/// The terrain for which to handle navigation.
		/// </summary>
		private readonly Terrain m_terrain;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a navigation map for the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public NavigationMap(Terrain terrain)
		{
			m_terrain = terrain;

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
		/// <param name="properties">A set of properties associated with the entity for which a
		/// path is to be found (can be null if irrelevant).</param>
		/// <returns>The path, as a list of points to traverse, or null if no path can be found.</returns>
		public Queue<Vector2> FindPath(Vector2 source, IEnumerable<Vector2> destinations, IDictionary<string,dynamic> properties)
		{
			// Determine the source and destination nodes for the pathfinding call.
			Vector2i sourceSquare = source.ToVector2i();
			NavigationNodeType sourceNode = m_nodeGrid[sourceSquare.Y, sourceSquare.X];

			List<NavigationNodeType> destinationNodes = destinations.Select(d =>
			{
				var s = d.ToVector2i();
				return m_nodeGrid[s.Y, s.X];
			}).ToList();

			// Run the pathfinder.
			LinkedList<NavigationNodeType> nodePath = AStarSearcher<NavigationNodeType>.FindPath(sourceNode, destinationNodes, properties);
			if(nodePath == null) return null;

			// Prepend the source node to the path found.
			nodePath.AddFirst(sourceNode);

			// Convert the path to a sequence of points at the centres of the nodes.
			var waypoints = new Queue<Vector2>(nodePath.Select(n => n.Position.ToVector2()));

			// Determine which of the possible actual destinations was actually selected,
			// and add it as the final waypoint.
			float bestDistanceSquared = float.MaxValue;
			Vector2? bestDestination = null;
			foreach(Vector2 d in destinations)
			{
				float distanceSquared = (d - waypoints.Last()).LengthSquared();
				if(distanceSquared < bestDistanceSquared)
				{
					bestDestination = d;
					bestDistanceSquared = distanceSquared;
				}
			}
			waypoints.Enqueue(bestDestination.Value);

			return waypoints;
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
