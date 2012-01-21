/***
 * game1666proto4: Edge.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// This enum represents the flag on an edge. An edge is currently part of the matching iff it is marked.
	/// </summary>
	enum MatchmakingEdgeFlag
	{
		UNMARKED,
		MARKED
	}
	
	/// <summary>
	/// An instance of this class represents an edge in the matchmaking graph.
	/// </summary>
	class MatchmakingEdge
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
		public MatchmakingEdgeFlag Flag { get; set; }

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
		public MatchmakingEdge(int source, int destination, int weight)
		{
			Source = source;
			Destination = destination;
			Weight = weight;

			Flag = MatchmakingEdgeFlag.UNMARKED;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not this edge is equal to another object.
		/// </summary>
		/// <param name="rhs">The other object.</param>
		/// <returns>true, if the other object is a edge equal to this one, or false otherwise.</returns>
		public override bool Equals(object rhs)
		{
			if(rhs is MatchmakingEdge) return Equals((MatchmakingEdge)rhs);
			else return false;
		}

		/// <summary>
		/// Tests whether or not this edge is equal to another one.
		/// </summary>
		/// <param name="rhs">The other edge.</param>
		/// <returns>true, if the two edges are equal, or false otherwise.</returns>
		public bool Equals(MatchmakingEdge rhs)
		{
			return Source == rhs.Source && Destination == rhs.Destination && Weight == rhs.Weight;
		}

		/// <summary>
		/// Returns the hash code for this edge.
		/// </summary>
		/// <returns>The hash code for this edge.</returns>
		public override int GetHashCode()
		{
			// Note: This is the type of hash recommended in Effective Java (the goodness of hash functions is language-independent).
			int hash = 17;
			hash = hash * 37 + Source.GetHashCode();
			hash = hash * 37 + Destination.GetHashCode();
			hash = hash * 37 + Weight.GetHashCode();
			return hash;
		}

		#endregion
	}
}
