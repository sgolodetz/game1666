/***
 * game1666proto4: Edge.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// This enum represents the flag on an edge. An edge is currently part of the matching iff it is marked.
	/// </summary>
	enum EdgeFlag
	{
		UNMARKED,
		MARKED
	}
	
	/// <summary>
	/// An instance of this class represents an edge in the matchmaking graph.
	/// </summary>
	class Edge
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The index of the destination node (the one in the right-hand column of the bipartite matchmaking graph).
		/// </summary>
		public int Destination { get; private set; }

		/// <summary>
		/// The flag on the edge, indicating whether or not it is currently part of the matching.
		/// </summary>
		public EdgeFlag Flag { get; set; }

		/// <summary>
		/// The index of the source node (the one in the left-hand column of the bipartite matchmaking graph).
		/// </summary>
		public int Source { get; private set; }

		/// <summary>
		/// The (positive) weight on the edge, indicating the mutual affinity of the source and destination nodes it joins.
		/// </summary>
		public int Weight { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an edge with the specified weight that joins the specified source and destination nodes in the graph.
		/// </summary>
		/// <param name="source">The index of the source node.</param>
		/// <param name="destination">The index of the destination node.</param>
		/// <param name="weight">The weight on the edge.</param>
		public Edge(int source, int destination, int weight)
		{
			Source = source;
			Destination = destination;
			Weight = weight;

			Flag = EdgeFlag.UNMARKED;
		}

		#endregion
	}
}
