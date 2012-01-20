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
		/// An instance of this class represents a path through the matchmaking graph.
		/// Paths are used when trying to improve the existing matching.
		/// </summary>
		private class Path
		{
			//#################### PROPERTIES ####################
			#region

			/// <summary>
			/// The edges in the path (irrespective of order), including the last edge added.
			/// </summary>
			public HashSet<Edge> Edges { get; set; }

			/// <summary>
			/// The last edge added to the path.
			/// </summary>
			public Edge LastEdge { get; set; }

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
		private List<Edge>[] m_destinationEdges;

		/// <summary>
		/// A table specifying the lists of edges connected to each of the source nodes.
		/// </summary>
		private List<Edge>[] m_sourceEdges;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Gets the sequence of edges that are currently part of the matching.
		/// </summary>
		public IEnumerable<Edge> MatchingEdges
		{
			get
			{
				foreach(List<Edge> edgeList in m_sourceEdges)
				{
					foreach(Edge edge in edgeList)
					{
						if(edge.Flag == EdgeFlag.MARKED)
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
			m_sourceEdges = new List<Edge>[sourceCount];
			for(int i = 0; i < sourceCount; ++i)
			{
				m_sourceEdges[i] = new List<Edge>();
			}

			m_destinationEdges = new List<Edge>[destinationCount];
			for(int i = 0; i < destinationCount; ++i)
			{
				m_destinationEdges[i] = new List<Edge>();
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
			var edge = new Edge(source, destination, weight);
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
			// TODO
		}

		/// <summary>
		/// Attempts to incrementally improve the existing matching.
		/// </summary>
		/// <returns>true, if an improved matching was generated, or false otherwise</returns>
		private bool ImproveMatch()
		{
			// TODO
			return false;
		}

		/// <summary>
		/// Makes a new path that augments an existing path by adding a new edge.
		/// </summary>
		/// <param name="path">The existing path.</param>
		/// <param name="edge">The new edge.</param>
		/// <returns>The augmented path.</returns>
		private static Path MakeAugmentedPath(Path path, Edge edge)
		{
			var edges = new HashSet<Edge>(path.Edges);
			edges.Add(edge);

			return new Path
			{
				Edges = edges,
				LastEdge = edge,
				Score = path.Score + edge.Weight * (edge.Flag == EdgeFlag.UNMARKED ? 1 : -1)
			};
		}

		/// <summary>
		/// Makes a new path that contains only a single edge.
		/// </summary>
		/// <param name="edge">The edge in question.</param>
		/// <returns>The constructed path.</returns>
		private static Path MakeSingletonPath(Edge edge)
		{
			var edges = new HashSet<Edge>();
			edges.Add(edge);

			return new Path
			{
				Edges = edges,
				LastEdge = edge,
				Score = edge.Weight * (edge.Flag == EdgeFlag.UNMARKED ? 1 : -1)
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
				if(m_sourceEdges[path.LastEdge.Source].Any(e => e.Flag == EdgeFlag.MARKED && !path.Edges.Contains(e)))
				{
					return false;
				}
			}
			else
			{
				// The last node in the path is a destination, so we need to check that no marked,
				// unseen destination -> source edge leads out of the destination of the last edge.
				if(m_destinationEdges[path.LastEdge.Destination].Any(e => e.Flag == EdgeFlag.MARKED && !path.Edges.Contains(e)))
				{
					return false;
				}
			}

			// If we get here, the path is both useful and valid, so we flip the flags on all the edges in the path.
			foreach(Edge edge in path.Edges)
			{
				edge.Flag = edge.Flag == EdgeFlag.UNMARKED ? EdgeFlag.MARKED : EdgeFlag.UNMARKED;
			}

			return true;
		}

		#endregion
	}
}
