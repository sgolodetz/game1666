/***
 * game1666proto4: EntityNavigationNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.AStar;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Navigation;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a node in the A* search space used for entity pathfinding.
	/// </summary>
	sealed class EntityNavigationNode : AStarNode<EntityNavigationNode>, INavigationNode<IPlaceableEntity,EntityNavigationNode>
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

		/// <summary>
		/// The terrain.
		/// </summary>
		private Terrain m_terrain;

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
		/// Constructs a blank entity navigation node. This must then be properly initialised using the Initialise method.
		/// </summary>
		public EntityNavigationNode()
		{}

		/// <summary>
		/// Initialises the node.
		/// </summary>
		/// <param name="position">The position of the node on the terrain.</param>
		/// <param name="nodeGrid">A grid of nodes for the other squares in the terrain.</param>
		/// <param name="terrain">The terrain.</param>
		public EntityNavigationNode Initialise(Vector2i position, EntityNavigationNode[,] nodeGrid, Terrain terrain)
		{
			m_nodeGrid = nodeGrid;
			m_position = position;
			m_terrain = terrain;
			return this;
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
			// Since we only allow 4-connected movement, the cost to the goal is at least
			// the Manhattan distance, so this is an admissible heuristic.
			H = destinations.Select(n => Math.Abs(m_position.X - n.m_position.X) + Math.Abs(m_position.Y - n.m_position.Y)).Min();
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
