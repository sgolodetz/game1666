/***
 * game1666proto4: AStarSearcher.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4.Common.AStar
{
	/// <summary>
	/// This class implements generic A* search.
	/// </summary>
	/// <typeparam name="T">The type of arbitrary data associated with the nodes used in the search.</typeparam>
	public static class AStarSearcher<T>
	{
		//#################### NESTED CLASSES ####################
		#region

		/// <summary>
		/// An instance of this class is used to compare AStarNode instances.
		/// </summary>
		private class AStarNodeComparer : IComparer<AStarNode<T>>
		{
			/// <summary>
			/// Compares one AStarNode to another.
			/// </summary>
			/// <param name="lhs">The left-hand operand of the comparison.</param>
			/// <param name="rhs">The right-hand operand of the comparison.</param>
			/// <returns>A -ve value if lhs is less than rhs, a +ve value if rhs is less than lhs, or 0 if they're equal.</returns>
			public int Compare(AStarNode<T> lhs, AStarNode<T> rhs)
			{
				return lhs.F.CompareTo(rhs.F);
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Finds a path from the specified source to the nearest of the specified destinations.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destinations">The destinations.</param>
		/// <returns>The path, as a list of nodes to traverse, or null if no path can be found.</returns>
		public static LinkedList<AStarNode<T>> FindPath(AStarNode<T> source, ICollection<AStarNode<T>> destinations)
		{
			var openList = new SortedSet<AStarNode<T>>(new AStarNodeComparer());
			var closedList = new HashSet<AStarNode<T>>();

			source.G = 0f;
			source.CalculateH(destinations);
			openList.Add(source);

			while(openList.Count != 0)
			{
				AStarNode<T> cur = openList.Min;
				if(destinations.Contains(cur))
				{
					return ConstructPath(cur);
				}

				openList.Remove(cur);
				closedList.Add(cur);

				foreach(AStarNode<T> neighbour in cur.Neighbours)
				{
					if(closedList.Contains(neighbour)) continue;

					float tentativeG = cur.G + cur.CostToNeighbour(neighbour);

					if(!openList.Contains(neighbour))
					{
						neighbour.G = tentativeG;
						neighbour.CalculateH(destinations);
						neighbour.From = cur;
						openList.Add(neighbour);
					}
					else if(tentativeG < neighbour.G)
					{
						neighbour.G = tentativeG;
						neighbour.From = cur;
					}
				}
			}

			return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Constructs the path leading to the specified destination by following
		/// the 'from' pointers back to the source. Note that the constructed path
		/// does not include the source itself.
		/// </summary>
		/// <param name="destination">The destination at the end of the path.</param>
		/// <returns>The nodes in the path as a list.</returns>
		private static LinkedList<AStarNode<T>> ConstructPath(AStarNode<T> destination)
		{
			var result = new LinkedList<AStarNode<T>>();

			AStarNode<T> cur = destination;
			while(cur.From != null)
			{
				result.AddFirst(cur);
				cur = cur.From;
			}

			return result;
		}

		#endregion
	}
}
