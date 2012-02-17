/***
 * game1666proto4: AStarSearcher.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.ADTs;

namespace game1666proto4.Common.AStar
{
	/// <summary>
	/// This class implements generic A* search.
	/// </summary>
	/// <typeparam name="T">The type of arbitrary data associated with the nodes used in the search.</typeparam>
	public static class AStarSearcher<T>
	{
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
			var openList = new PriorityQueue<AStarNode<T>, float, int?>(Comparer<float>.Default);
			var closedList = new HashSet<AStarNode<T>>();

			source.G = 0f;
			source.CalculateH(destinations);
			openList.Insert(source, source.F, null);

			while(openList.Count != 0)
			{
				AStarNode<T> cur = openList.Top.ID;
				if(destinations.Contains(cur))
				{
					return ConstructPath(cur);
				}

				openList.Pop();
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
						openList.Insert(neighbour, neighbour.F, null);
					}
					else if(tentativeG < neighbour.G)
					{
						neighbour.G = tentativeG;
						neighbour.From = cur;
						openList.UpdateKey(neighbour, neighbour.F);
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
