/***
 * game1666proto4: NavigationNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.AStar;
using game1666proto4.Common.Terrains;

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
		/// The occupancy map for the terrain over which pathfinding will take place.
		/// </summary>
		private IOccupancyMap m_occupancyMap;

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
		/// <param name="occupancyMap">The occupancy map for the terrain over which pathfinding will take place.</param>
		public NavigationNode(IOccupancyMap occupancyMap)
		{
			m_occupancyMap = occupancyMap;
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
