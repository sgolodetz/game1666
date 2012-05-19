/***
 * game1666: ModelEntityNavigationNode.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using game1666.Common.AStar;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class represents a node in the A* search space used for entity pathfinding.
	/// </summary>
	sealed class ModelEntityNavigationNode : AStarNode<ModelEntityNavigationNode>, INavigationNode<ModelEntity,ModelEntityNavigationNode>
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
		private ModelEntityNavigationNode[,] m_nodeGrid;

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
		/// The entity occupying the grid square for which this is the navigation node (if any), or null otherwise.
		/// </summary>
		public ModelEntity OccupyingEntity { get; set; }

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
		public ModelEntityNavigationNode()
		{}

		/// <summary>
		/// Initialises the node.
		/// </summary>
		/// <param name="position">The position of the node on the terrain.</param>
		/// <param name="nodeGrid">A grid of nodes for the other squares in the terrain.</param>
		/// <param name="terrain">The terrain.</param>
		/// <returns>The node itself.</returns>
		public ModelEntityNavigationNode Initialise(Vector2i position, ModelEntityNavigationNode[,] nodeGrid, Terrain terrain)
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
		public override void CalculateH(ICollection<ModelEntityNavigationNode> destinations)
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
		public override float CostToNeighbour(ModelEntityNavigationNode neighbour)
		{
			switch(ClassifyOccupyingEntity() + neighbour.ClassifyOccupyingEntity())
			{
				case 2:
				{
					// None - None
					return 10f;
				}
				case 3:
				{
					// None - Placeable: Costly to prevent placeables being used as cut-throughs.
					return 100f;
				}
				case 4:
				{
					// Placeable - Placeable: Costly to prevent placeables being used as cut-throughs.
					return 100f;
				}
				case 5:
				{
					// None - Road Segment
					return 10f;
				}
				case 6:
				{
					// Placeable - Road Segment: Costly to prevent placeables being used as cut-throughs.
					return 100f;
				}
				default:
				{
					// Road Segment - Road Segment
					if(Math.Abs(Position.X - neighbour.Position.X) + Math.Abs(Position.Y - neighbour.Position.Y) == 1)
					{
						// Following a (4-connected) road should have the lowest cost.
						return 1f;
					}
					else
					{
						// Walking diagonally from road segment to road segment should be comparatively cheap,
						// but not as good as following the road itself.
						return 5f;
					}
				}
			}
		}

		/// <summary>
		/// Determines the neighbours of this node in the search space.
		/// </summary>
		/// <param name="properties">A set of properties associated with the entity for which a path is to be found (can be null if irrelevant).</param>
		/// <returns>The neighbours of the node.</returns>
		public override IEnumerable<ModelEntityNavigationNode> Neighbours(IDictionary<string,dynamic> properties)
		{
			MobileBlueprint blueprint = BlueprintManager.GetBlueprint(properties["Blueprint"]);

			foreach(Vector2i neighbourPos in NeighbourPositions)
			{
				if(0 <= neighbourPos.X && neighbourPos.X < m_nodeGrid.GetLength(1) &&
				   0 <= neighbourPos.Y && neighbourPos.Y < m_nodeGrid.GetLength(0))
				{
					ModelEntityNavigationNode neighbour = m_nodeGrid[neighbourPos.Y, neighbourPos.X];
					ModelEntity neighbourEntity = neighbour.OccupyingEntity;
					IPlaceableComponent neighbourPlaceable = neighbourEntity != null ? neighbourEntity.GetComponent(ModelEntityComponentGroups.EXTERNAL) : null;

					if(Math.Abs(neighbour.m_altitude - m_altitude) <= blueprint.MaxAltitudeChange &&
					   (neighbourEntity == null || (neighbourPlaceable != null && neighbourPlaceable.Entrances.Contains(neighbourPos))))
					{
						yield return neighbour;
					}
				}
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Classifies the entity occupying this node as being either a road segment, a different placeable entity, or non-existent.
		/// </summary>
		/// <returns>An int denoting the type of entity occupying the node.</returns>
		private int ClassifyOccupyingEntity()
		{
			// Note: The values are chosen to be powers of two deliberately - see their use in CostToNeighbour.
			if(OccupyingEntity != null)
			{
				if(OccupyingEntity.HasComponent("GameModel/External", "Traversable")) return 4;
				else return 2;
			}
			else return 1;
		}

		#endregion
	}
}
