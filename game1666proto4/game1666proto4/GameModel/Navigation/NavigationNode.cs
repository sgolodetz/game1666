/***
 * game1666proto4: NavigationNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.AStar;
using game1666proto4.Common.Maths;

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of this class represents a node in the A* search space used for pathfinding.
	/// </summary>
	sealed class NavigationNode : AStarNode<NavigationNode>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A grid of nodes for the other squares in the terrain.
		/// </summary>
		private NavigationNode[,] m_nodeGrid;

		/// <summary>
		/// The occupancy map for the terrain over which pathfinding will take place.
		/// </summary>
		private IOccupancyMap m_occupancyMap;

		/// <summary>
		/// The position of the node on the terrain.
		/// </summary>
		private Vector2i m_position;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The neighbours of this node in the search space.
		/// </summary>
		public override IEnumerable<NavigationNode> Neighbours
		{
			get
			{
				// TODO
				return null;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a navigation node to be used in pathfinding over a terrain.
		/// </summary>
		/// <param name="position">The position of the node on the terrain.</param>
		/// <param name="occupancyMap">The occupancy map for the terrain over which pathfinding will take place.</param>
		/// <param name="nodeGrid">A grid of nodes for the other squares in the terrain.</param>
		public NavigationNode(Vector2i position, IOccupancyMap occupancyMap, NavigationNode[,] nodeGrid)
		{
			m_nodeGrid = nodeGrid;
			m_occupancyMap = occupancyMap;
			m_position = position;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Calculates the estimated cost of the sub-path from this node to the goal.
		/// </summary>
		/// <param name="destinations">The destination nodes.</param>
		public override void CalculateH(ICollection<NavigationNode> destinations)
		{
			// TODO
		}

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public override float CostToNeighbour(NavigationNode neighbour)
		{
			// TODO
			return 0f;
		}

		#endregion
	}
}
