/***
 * game1666: AStarSearcher.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.ADTs;

namespace game1666.Common.AStar
{
	/// <summary>
	/// This class implements generic A* search.
	/// </summary>
	/// <typeparam name="NodeType">The type of node used in the search.</typeparam>
	public static class AStarSearcher<NodeType> where NodeType : AStarNode<NodeType>
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Finds a path from the specified source to the nearest of the specified destinations.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destinations">The destinations.</param>
		/// <param name="entityProperties">The properties of the entity for which a path is to be found (can be null if irrelevant).</param>
		/// <returns>The path, as a list of nodes to traverse, or null if no path can be found.</returns>
		public static LinkedList<NodeType> FindPath(NodeType source, ICollection<NodeType> destinations, IDictionary<string,dynamic> entityProperties)
		{
			// The open list stores the "fringe" of the search - these are the nodes we're currently exploring.
			var openList = new PriorityQueue<NodeType, float, int?>(Comparer<float>.Default);

			// The closed list stores the nodes we've finished processing - we need to ensure that we don't revisit them.
			var closedList = new HashSet<NodeType>();

			// Add the source node to the open list.
			source.G = 0f;
			source.CalculateH(destinations);
			source.From = null;
			openList.Insert(source, source.F, null);

			// While there are remaining nodes in the open list:
			while(openList.Count != 0)
			{
				// Check whether the node on the open list with the minimum estimated total cost
				// is one of the destinations. If so, construct and return the path to it.
				// (This is done by following the 'from' pointers back to the source node.)
				NodeType cur = openList.Top.ID;
				if(destinations.Contains(cur))
				{
					return ConstructPath(cur);
				}

				// If the top node on the open list is not a destination node, move it to the closed list.
				openList.Pop();
				closedList.Add(cur);

				// Then, process each of its neighbours in turn.
				foreach(NodeType neighbour in cur.Neighbours(entityProperties))
				{
					// If we've already finished processing the neighbour, ignore it -
					// the route to it through this node is provably no better.
					if(closedList.Contains(neighbour)) continue;

					// Calculate the cost from the source to the neighbour through the current node.
					float tentativeG = cur.G + cur.CostToNeighbour(neighbour);

					if(!openList.Contains(neighbour))
					{
						// If the node has not yet been added to the open list, this is the best path
						// to it we've seen so far, so fill it in and add it to the open list.
						neighbour.G = tentativeG;
						neighbour.CalculateH(destinations);
						neighbour.From = cur;
						openList.Insert(neighbour, neighbour.F, null);
					}
					else if(tentativeG < neighbour.G)
					{
						// If the node is already on the open list, but this is a better path, update it.
						neighbour.G = tentativeG;
						neighbour.From = cur;
						openList.UpdateKey(neighbour, neighbour.F);
					}
				}
			}

			// If the open list empties without us reaching a destination node, no path exists.
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
		private static LinkedList<NodeType> ConstructPath(NodeType destination)
		{
			var result = new LinkedList<NodeType>();

			NodeType cur = destination;
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
