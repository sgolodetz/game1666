/***
 * game1666proto4: Matchmaker.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// This class is used to match resource offers with resource requests.
	/// </summary>
	/// <typeparam name="OfferType">The type used to specify the offers.</typeparam>
	/// <typeparam name="RequestType">The type used to specify the requests.</typeparam>
	static class Matchmaker<OfferType, RequestType>
		where OfferType : IMatchmakingOffer
		where RequestType : IMatchmakingRequest<OfferType>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A list of outstanding offers to supply resources.
		/// </summary>
		private static IList<OfferType> s_offers = new List<OfferType>();

		/// <summary>
		/// A list of outstanding requests to consume resources.
		/// </summary>
		private static IList<RequestType> s_requests = new List<RequestType>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Attempts to match up the offers and requests currently in the matchmaker.
		/// Informs parties that are successfully matched.
		/// </summary>
		public static void Match()
		{
			// Build a bipartite matchmaking graph with suitably weighted edges between offers and requests.
			// The weights are determined by quantifying the extent to which offers satisfy requests.
			var graph = new MatchmakingGraph(s_offers.Count, s_requests.Count);
			for(int source = 0; source < s_offers.Count; ++source)
			{
				OfferType offer = s_offers[source];
				for(int destination = 0; destination < s_requests.Count; ++destination)
				{
					RequestType request = s_requests[destination];
					int weight = request.QuantifyOffer(offer);
					if(weight > 0)
					{
						graph.AddEdge(source, destination, weight);
					}
				}
			}

			// Run the graph matching algorithm to find an optimal matching.
			graph.FindBestMatching();

			// TODO: Inform the relevant parties that they have been successfully matched.

			// Clear all unresolved offers and requests - they will have to be resubmitted to the matchmaker next time.
			Reset();
		}

		/// <summary>
		/// Adds an offer of resources to the matchmaker.
		/// </summary>
		/// <param name="offer">The offer.</param>
		public static void PostOffer(OfferType offer)
		{
			s_offers.Add(offer);
		}

		/// <summary>
		/// Adds a request for resources to the matchmaker.
		/// </summary>
		/// <param name="request">The request.</param>
		public static void PostRequest(RequestType request)
		{
			s_requests.Add(request);
		}

		/// <summary>
		/// Resets the matchmaker (clears all existing offers and requests).
		/// </summary>
		public static void Reset()
		{
			s_offers.Clear();
			s_requests.Clear();
		}

		#endregion
	}
}
