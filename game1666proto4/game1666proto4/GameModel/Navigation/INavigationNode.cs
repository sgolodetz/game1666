/***
 * game1666proto4: INavigationNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Maths;

namespace game1666proto4.GameModel.Navigation
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

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Initialises the node.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="nodeGrid"></param>
		/// <returns></returns>
		NodeType Initialise(Vector2i position, NodeType[,] nodeGrid);

		#endregion
	}
}
