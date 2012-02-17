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
	/// <typeparam name="T">The type of arbitrary data associated with the node.</typeparam>
	public abstract class AStarNode<T> : IEquatable<AStarNode<T>>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Arbitrary data associated with the node (e.g. its position).
		/// </summary>
		public T Data { get; protected set; }

		/// <summary>
		/// The estimated total cost of the path through this node.
		/// </summary>
		public float F { get { return G + H; } }

		/// <summary>
		/// A pointer to the preceding node in the path (initially null).
		/// Used for finding the path at the end of the search.
		/// </summary>
		public AStarNode<T> From { get; set; }

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
		public abstract IEnumerable<AStarNode<T>> Neighbours { get; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Calculates the estimated cost of the sub-path from this node to the goal.
		/// </summary>
		/// <param name="destinations">The destination nodes.</param>
		public abstract void CalculateH(ICollection<AStarNode<T>> destinations);

		/// <summary>
		/// Determines the cost of going from this node to the specified neighbouring node.
		/// </summary>
		/// <param name="neighbour">The neighbouring node.</param>
		/// <returns>The cost of going from this node to the specified neighbouring node.</returns>
		public abstract float CostToNeighbour(AStarNode<T> neighbour);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines whether or not this AStarNode is equal to another one.
		/// </summary>
		/// <param name="rhs">The other AStarNode.</param>
		/// <returns>true, if the two nodes are equal, or false otherwise.</returns>
		public bool Equals(AStarNode<T> rhs)
		{
			return object.Equals(this, rhs);
		}

		#endregion
	}
}
