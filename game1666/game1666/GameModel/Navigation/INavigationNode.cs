/***
 * game1666: INavigationNode.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Maths;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Navigation
{
	/// <summary>
	/// An instance of a class implementing this interface represents a node in the A* search space used for pathfinding.
	/// </summary>
	/// <typeparam name="PlaceableEntityType">The type of entity that can be placed on the terrain.</typeparam>
	/// <typeparam name="NodeType">The actual derived node type.</typeparam>
	interface INavigationNode<PlaceableEntityType,NodeType>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity occupying the grid square (if any), or null otherwise.
		/// </summary>
		PlaceableEntityType OccupyingEntity { get; set; }

		/// <summary>
		/// The position of the node on the terrain.
		/// </summary>
		Vector2i Position { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Initialises the node.
		/// </summary>
		/// <param name="position">The position of the node on the terrain.</param>
		/// <param name="nodeGrid">A grid of nodes for the other squares in the terrain.</param>
		/// <param name="terrain">The terrain.</param>
		/// <returns>The node itself.</returns>
		NodeType Initialise(Vector2i position, NodeType[,] nodeGrid, Terrain terrain);

		#endregion
	}
}
