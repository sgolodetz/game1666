/***
 * game1666proto4: Matchmaker.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// An instance of this class is used to match resource offers with resource requests.
	/// </summary>
	/// <typeparam name="OfferType">The type used to specify the offers.</typeparam>
	/// <typeparam name="RequestType">The type used to specify the requests.</typeparam>
	class Matchmaker<OfferType, RequestType>
		where OfferType : IMatchmakingOffer
		where RequestType : IMatchmakingRequest<OfferType>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A list of outstanding offers to supply resources.
		/// </summary>
		private IList<OfferType> m_offers = new List<OfferType>();

		/// <summary>
		/// A list of outstanding requests to consume resources.
		/// </summary>
		private IList<RequestType> m_requests = new List<RequestType>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Attempts to match up the offers and requests currently in the matchmaker.
		/// Informs parties that are successfully matched.
		/// </summary>
		public void Match()
		{
			// Build a bipartite matchmaking graph with suitably weighted edges between offers and requests.
			// The weights are determined by quantifying the extent to which offers satisfy requests.
			var graph = new MatchmakingGraph(m_offers.Count, m_requests.Count);
			for(int source = 0; source < m_offers.Count; ++source)
			{
				OfferType offer = m_offers[source];
				for(int destination = 0; destination < m_requests.Count; ++destination)
				{
					RequestType request = m_requests[destination];
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
		public void PostOffer(OfferType offer)
		{
			m_offers.Add(offer);
		}

		/// <summary>
		/// Adds a request for resources to the matchmaker.
		/// </summary>
		/// <param name="request">The request.</param>
		public void PostRequest(RequestType request)
		{
			m_requests.Add(request);
		}

		/// <summary>
		/// Resets the matchmaker (clears all existing offers and requests).
		/// </summary>
		public void Reset()
		{
			m_offers.Clear();
			m_requests.Clear();
		}

		#endregion
	}
}
