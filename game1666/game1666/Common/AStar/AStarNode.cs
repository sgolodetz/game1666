/***
 * game1666: AStarNode.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666.Common.AStar
{
	/// <summary>
	/// An instance of a class deriving from this one represents a node in an A* search space.
	/// </summary>
	/// <typeparam name="NodeType">The actual derived node type.</typeparam>
	abstract class AStarNode<NodeType> : IEquatable<NodeType>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The estimated total cost of the path through this node.
		/// </summary>
		public float F { get { return G + H; } }

		/// <summary>
		/// A pointer to the preceding node in the path (initially null).
		/// Used for finding the path at the end of the search.
		/// </summary>
		public NodeType From { get; set; }

		/// <summary>
		/// The cost of the sub-path from the source to this node.
		/// </summary>
		public float G { get; set; }

		/// <summary>
		/// The estimated cost of the sub-path from this node to the nearest goal.
		/// </summary>
		public float H { get; protected set; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Calculates the estimated cost of the sub-path from this node to the nearest goal.
		/// </summary>
		/// <param name="destinations">The destination nodes.</param>
		public abstract void CalculateH(ICollection<NodeType> destinations);

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public abstract float CostToNeighbour(NodeType neighbour);

		/// <summary>
		/// Determines the neighbours of this node in the search space.
		/// </summary>
		/// <param name="properties">A set of properties associated with the entity
		/// for which a path is to be found (can be null if irrelevant).</param>
		/// <returns>The neighbours of the node.</returns>
		public abstract IEnumerable<NodeType> Neighbours(IDictionary<string,dynamic> properties);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines whether or not this AStarNode is equal to another one.
		/// </summary>
		/// <param name="rhs">The other node.</param>
		/// <returns>true, if the two nodes are equal, or false otherwise.</returns>
		public bool Equals(NodeType rhs)
		{
			return object.Equals(this, rhs);
		}

		#endregion
	}
}
