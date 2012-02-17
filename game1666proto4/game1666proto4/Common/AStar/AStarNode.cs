/***
 * game1666proto4: AStarNode.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666proto4.Common.AStar
{
	/// <summary>
	/// An instance of a class deriving from this one represents a node in an A* search space.
	/// </summary>
	/// <typeparam name="NodeType">The actual derived node type.</typeparam>
	public abstract class AStarNode<NodeType> : IEquatable<NodeType>
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
		/// The estimated cost of the sub-path from this node to the goal.
		/// </summary>
		public float H { get; protected set; }

		/// <summary>
		/// The neighbours of this node in the search space.
		/// </summary>
		public abstract IEnumerable<NodeType> Neighbours { get; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Calculates the estimated cost of the sub-path from this node to the goal.
		/// </summary>
		/// <param name="destinations">The destination nodes.</param>
		public abstract void CalculateH(ICollection<NodeType> destinations);

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public abstract float CostToNeighbour(NodeType neighbour);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines whether or not this AStarNode is equal to another node.
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
