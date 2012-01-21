/***
 * game1666proto4: MatchmakingGraph.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// An instance of this class represents a weighted bipartite graph used for matchmaking.
	/// The graph is divided into two columns: "source" nodes on the left and "destination"
	/// nodes on the right. The goal of the matchmaking algorithm is to find a set of
	/// (source, destination) node pairs such that the sum of the weights on the edges
	/// joining the pairs is maximal.
	/// </summary>
	sealed class MatchmakingGraph
	{
		//#################### NESTED CLASSES ####################
		#region

		/// <summary>
		/// An instance of this class represents an alternating path through the matchmaking graph
		/// (that is, one whose edges alternate between having a marked and an unmarked flag).
		/// Alternating paths are used when trying to improve the existing matching.
		/// </summary>
		private class Path
		{
			//#################### PROPERTIES ####################
			#region

			/// <summary>
			/// The edges in the path (irrespective of order), including the last edge added.
			/// </summary>
			public HashSet<MatchmakingEdge> Edges { get; set; }

			/// <summary>
			/// The last edge added to the path.
			/// </summary>
			public MatchmakingEdge LastEdge { get; set; }

			/// <summary>
			/// The current "score" of the path. This equals the result of subtracting the
			/// sum of the weights on the marked path edges from the sum of the weights on
			/// the unmarked edges. A path with strictly positive score can potentially be
			/// used to improve the current matching by flipping the marked/unmarked flags
			/// of all of its edges.
			/// </summary>
			public int Score { get; set; }

			#endregion
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A table specifying the lists of edges connected to each of the destination nodes.
		/// </summary>
		private List<MatchmakingEdge>[] m_destinationEdges;

		/// <summary>
		/// A table specifying the lists of edges connected to each of the source nodes.
		/// </summary>
		private List<MatchmakingEdge>[] m_sourceEdges;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Gets the sequence of edges that are currently part of the matching.
		/// </summary>
		public IEnumerable<MatchmakingEdge> MatchingEdges
		{
			get
			{
				foreach(List<MatchmakingEdge> edgeList in m_sourceEdges)
				{
					foreach(MatchmakingEdge edge in edgeList)
					{
						if(edge.Flag == MatchmakingEdgeFlag.MARKED)
						{
							yield return edge;
						}
					}
				}
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new bipartite matchmaking graph with the specified numbers of source and destination nodes.
		/// </summary>
		/// <param name="sourceCount">The number of source nodes in the graph.</param>
		/// <param name="destinationCount">The number of destination nodes in the graph.</param>
		public MatchmakingGraph(int sourceCount, int destinationCount)
		{
			m_sourceEdges = new List<MatchmakingEdge>[sourceCount];
			for(int i = 0; i < sourceCount; ++i)
			{
				m_sourceEdges[i] = new List<MatchmakingEdge>();
			}

			m_destinationEdges = new List<MatchmakingEdge>[destinationCount];
			for(int i = 0; i < destinationCount; ++i)
			{
				m_destinationEdges[i] = new List<MatchmakingEdge>();
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds to the graph a new edge with the specified weight that joins the specified source and destination nodes.
		/// </summary>
		/// <param name="source">The source node.</param>
		/// <param name="destination">The destination node.</param>
		/// <param name="weight">The weight on the edge.</param>
		public void AddEdge(int source, int destination, int weight)
		{
			var edge = new MatchmakingEdge(source, destination, weight);
			m_sourceEdges[source].Add(edge);
			m_destinationEdges[destination].Add(edge);
		}

		/// <summary>
		/// Finds an optimal matching, i.e. one of maximal overall weight.
		/// </summary>
		public void FindBestMatching()
		{
			FindInitialMatching();

			while(ImproveMatch())
			{
				// Keep iterating until the match can no longer be improved.
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Finds an initial matching. This is an optimization rather than being strictly necessary.
		/// The only real requirement on this method is that it should not generate an invalid matching.
		/// </summary>
		private void FindInitialMatching()
		{
			// The algorithm used here is to initially match each source node to the
			// first unused destination node to which it is connected.
			var used = new HashSet<int>();
			foreach(List<MatchmakingEdge> edgeList in m_sourceEdges)
			{
				foreach (MatchmakingEdge edge in edgeList)
				{
					if (!used.Contains(edge.Source) && !used.Contains(edge.Destination))
					{
						edge.Flag = MatchmakingEdgeFlag.MARKED;
						used.Add(edge.Source);
						used.Add(edge.Destination);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Attempts to incrementally improve the existing matching.
		/// </summary>
		/// <returns>true, if an improved matching was generated, or false otherwise</returns>
		private bool ImproveMatch()
		{
			var pathQueue = new Queue<Path>();

			// Initialise the queue.
			foreach(List<MatchmakingEdge> edgeList in m_sourceEdges)
			{
				// If there is a marked edge leading out of this source node, use it and ignore all
				// the other edges (since there can be no marked edge leading out of the start of
				// the path that is not itself part of the path, if the path is to be usable later).
				// Otherwise, queue up paths for each edge leading out of this source node.
				MatchmakingEdge marked = edgeList.SingleOrDefault(e => e.Flag == MatchmakingEdgeFlag.MARKED);
				if(marked != null)
				{
					pathQueue.Enqueue(MakeSingletonPath(marked));
				}
				else
				{
					foreach(MatchmakingEdge edge in edgeList)
					{
						pathQueue.Enqueue(MakeSingletonPath(edge));
					}
				}
			}

			// Run the breadth-first search to find a path that can be used to improve the matching.
			while(pathQueue.Count > 0)
			{
				Path path = pathQueue.Dequeue();

				// Try and use the path we just dequeued to improve the matching. If it's usable,
				// this iteration of the improvement process succeeded and we're done.
				if(UsePath(path))
				{
					return true;
				}

				// If the path as it stands is not usable, enqueue all possible derived paths with one extra edge.
				// Note that we're only interested in *alternating* paths - that is, ones whose edge flags switch
				// between marked and unmarked - so we only consider appending edges with the opposite flag to that
				// of the last edge in the path. In order to look up the relevant edges, we first have to determine
				// whether the last node in the path is a source or a destination.
				if(path.Edges.Count % 2 == 0)
				{
					// The last node in the path is a source, so we're looking for an unseen source -> destination
					// edge with the opposite flag to the last edge.
					foreach(MatchmakingEdge e in m_sourceEdges[path.LastEdge.Source].Where(e => e.Flag != path.LastEdge.Flag && !path.Edges.Contains(e)))
					{
						pathQueue.Enqueue(MakeAugmentedPath(path, e));
					}
				}
				else
				{
					// The last node in the path is a destination, so we're looking for an unseen destination -> source
					// edge with the opposite flag to the last edge.
					foreach(MatchmakingEdge e in m_destinationEdges[path.LastEdge.Destination].Where(e => e.Flag != path.LastEdge.Flag && !path.Edges.Contains(e)))
					{
						pathQueue.Enqueue(MakeAugmentedPath(path, e));
					}
				}
			}

			// If the queue empties without our having found a suitable path with which to improve the matching,
			// this iteration of the improvement process failed and we're done. In practice, this also means we
			// have found an optimal matching overall.
			return false;
		}

		/// <summary>
		/// Makes a new path that augments an existing path by adding a new edge.
		/// </summary>
		/// <param name="path">The existing path.</param>
		/// <param name="edge">The new edge.</param>
		/// <returns>The augmented path.</returns>
		private static Path MakeAugmentedPath(Path path, MatchmakingEdge edge)
		{
			var edges = new HashSet<MatchmakingEdge>(path.Edges);
			edges.Add(edge);

			return new Path
			{
				Edges = edges,
				LastEdge = edge,
				Score = path.Score + edge.Weight * (edge.Flag == MatchmakingEdgeFlag.UNMARKED ? 1 : -1)
			};
		}

		/// <summary>
		/// Makes a new path that contains only a single edge.
		/// </summary>
		/// <param name="edge">The edge in question.</param>
		/// <returns>The constructed path.</returns>
		private static Path MakeSingletonPath(MatchmakingEdge edge)
		{
			var edges = new HashSet<MatchmakingEdge>();
			edges.Add(edge);

			return new Path
			{
				Edges = edges,
				LastEdge = edge,
				Score = edge.Weight * (edge.Flag == MatchmakingEdgeFlag.UNMARKED ? 1 : -1)
			};
		}

		/// <summary>
		/// Checks whether the specified path is usable, and flips the flags of all the edges in the path if so.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns>true, if the path was used, or false otherwise</returns>
		private bool UsePath(Path path)
		{
			// If using the path would not improve our matching, early out.
			if(path.Score <= 0)
			{
				return false;
			}

			// If using the path would improve our matching, check that we actually *can* use it.
			// The criterion for this is that there is no marked edge that leads out of the start
			// or end nodes of the path that is not already in the path. This is guaranteed for
			// the start node by the design of the algorithm, but we need to check for the end node.
			// In order to do this, we first check whether the last node is a source or a destination.
			if(path.Edges.Count % 2 == 0)
			{
				// The last node in the path is a source, so we need to check that no marked,
				// unseen source -> destination edge leads out of the source of the last edge.
				if(m_sourceEdges[path.LastEdge.Source].Any(e => e.Flag == MatchmakingEdgeFlag.MARKED && !path.Edges.Contains(e)))
				{
					return false;
				}
			}
			else
			{
				// The last node in the path is a destination, so we need to check that no marked,
				// unseen destination -> source edge leads out of the destination of the last edge.
				if(m_destinationEdges[path.LastEdge.Destination].Any(e => e.Flag == MatchmakingEdgeFlag.MARKED && !path.Edges.Contains(e)))
				{
					return false;
				}
			}

			// If we get here, the path is both useful and valid, so we flip the flags on all the edges in the path.
			foreach(MatchmakingEdge edge in path.Edges)
			{
				edge.Flag = edge.Flag == MatchmakingEdgeFlag.UNMARKED ? MatchmakingEdgeFlag.MARKED : MatchmakingEdgeFlag.UNMARKED;
			}

			return true;
		}

		#endregion
	}
}
