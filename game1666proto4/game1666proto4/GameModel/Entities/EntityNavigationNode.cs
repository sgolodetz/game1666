﻿/***
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
		/// The average altitude of the terrain grid square represented by the node.
		/// </summary>
		private float m_altitude;

		/// <summary>
		/// A grid of nodes for the other squares in the terrain.
		/// </summary>
		private EntityNavigationNode[,] m_nodeGrid;

		/// <summary>
		/// The terrain.
		/// </summary>
		private Terrain m_terrain;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The positions of potential neighbours of the node in the search space.
		/// </summary>
		private IEnumerable<Vector2i> NeighbourPositions
		{
			get
			{
				int x = Position.X, y = Position.Y;
				yield return new Vector2i(x-1, y-1);
				yield return new Vector2i(x, y-1);
				yield return new Vector2i(x+1, y-1);
				yield return new Vector2i(x-1, y);
				yield return new Vector2i(x+1, y);
				yield return new Vector2i(x-1, y+1);
				yield return new Vector2i(x, y+1);
				yield return new Vector2i(x+1, y+1);
			}
		}

		/// <summary>
		/// The neighbours of this node in the search space.
		/// </summary>
		public override IEnumerable<EntityNavigationNode> Neighbours
		{
			get
			{
				const float ALTITUDE_CHANGE_THRESHOLD = 5f;

				foreach(Vector2i neighbourPosition in NeighbourPositions)
				{
					if(0 <= neighbourPosition.X && neighbourPosition.X < m_nodeGrid.GetLength(1) &&
					   0 <= neighbourPosition.Y && neighbourPosition.Y < m_nodeGrid.GetLength(0))
					{
						EntityNavigationNode neighbour = m_nodeGrid[neighbourPosition.Y, neighbourPosition.X];
						if(Math.Abs(neighbour.m_altitude - m_altitude) <= ALTITUDE_CHANGE_THRESHOLD)
						{
							yield return neighbour;
						}
					}
				}
			}
		}

		/// <summary>
		/// The entity occupying the grid square for which this is the navigation node (if any), or null otherwise.
		/// </summary>
		public IPlaceableEntity OccupyingEntity { get; set; }

		/// <summary>
		/// The position of the node on the terrain.
		/// </summary>
		public Vector2i Position { get; private set; }

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
			Position = position;
			m_nodeGrid = nodeGrid;
			m_terrain = terrain;

			m_altitude = m_terrain.DetermineAverageAltitude(position);

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
			// The cost to the goal is at least the straight-line distance between the source and
			// the nearest destination, so this is an admissible heuristic.
			H = destinations.Select(n => (Position - n.Position).Length()).Min();
		}

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public override float CostToNeighbour(EntityNavigationNode neighbour)
		{
			if(OccupyingEntity is RoadSegment && neighbour.OccupyingEntity is RoadSegment)
			{
				return 1f;
			}
			else
			{
				return 2f;
			}
		}

		#endregion
	}
}
