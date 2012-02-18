/***
 * game1666proto4: EntityNavigationNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.AStar;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Navigation;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a node in the A* search space used for entity pathfinding.
	/// </summary>
	sealed class EntityNavigationNode : AStarNode<EntityNavigationNode>, IOccupancyHolder<IPlaceableEntity>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A grid of nodes for the other squares in the terrain.
		/// </summary>
		private EntityNavigationNode[,] m_nodeGrid;

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
		public override IEnumerable<EntityNavigationNode> Neighbours
		{
			get
			{
				// TODO
				return null;
			}
		}

		/// <summary>
		/// The entity occupying the grid square for which this is the navigation node (if any), or null otherwise.
		/// </summary>
		public IPlaceableEntity OccupyingEntity { get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity navigation node to be used in pathfinding over a terrain.
		/// </summary>
		/// <param name="position">The position of the node on the terrain.</param>
		/// <param name="nodeGrid">A grid of nodes for the other squares in the terrain.</param>
		public EntityNavigationNode(Vector2i position, EntityNavigationNode[,] nodeGrid)
		{
			m_nodeGrid = nodeGrid;
			m_position = position;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Calculates the estimated cost of the sub-path from this node to the goal.
		/// </summary>
		/// <param name="destinations">The destination nodes.</param>
		public override void CalculateH(ICollection<EntityNavigationNode> destinations)
		{
			// TODO
		}

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public override float CostToNeighbour(EntityNavigationNode neighbour)
		{
			// TODO
			return 0f;
		}

		#endregion
	}
}
