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
	class Matchmaker<OfferType, RequestType> where RequestType : IMatchmakingRequest<OfferType>
	{
		//#################### NESTED STRUCTS ####################
		#region

		/// <summary>
		/// An instance of this struct is used to pair an offer with its source. The
		/// source should not be stored in the offer itself for dependency reasons.
		/// </summary>
		private struct SourcedOffer
		{
			//#################### PRIVATE VARIABLES ####################
			#region

			/// <summary>
			/// The offer.
			/// </summary>
			private readonly OfferType m_offer;

			/// <summary>
			/// The source of the offer.
			/// </summary>
			private readonly IMatchmakingEntity<OfferType, RequestType> m_source;

			#endregion

			//#################### PROPERTIES ####################
			#region

			/// <summary>
			/// The offer.
			/// </summary>
			public OfferType Offer { get { return m_offer; } }

			/// <summary>
			/// The source of the offer.
			/// </summary>
			public IMatchmakingEntity<OfferType, RequestType> Source { get { return m_source; } }

			#endregion

			//#################### CONSTRUCTORS ####################
			#region

			/// <summary>
			/// Constructs a new sourced offer.
			/// </summary>
			/// <param name="offer">The offer.</param>
			/// <param name="source">The source of the offer.</param>
			public SourcedOffer(OfferType offer, IMatchmakingEntity<OfferType, RequestType> source)
			{
				m_offer = offer;
				m_source = source;
			}

			#endregion
		}

		/// <summary>
		/// An instance of this struct is used to pair a request with its source. The
		/// source should not be stored in the request itself for dependency reasons.
		/// </summary>
		private struct SourcedRequest
		{
			//#################### PRIVATE VARIABLES ####################
			#region

			/// <summary>
			/// The request.
			/// </summary>
			private readonly RequestType m_request;

			/// <summary>
			/// The source of the request.
			/// </summary>
			private readonly IMatchmakingEntity<OfferType, RequestType> m_source;

			#endregion

			//#################### PROPERTIES ####################
			#region

			/// <summary>
			/// The request.
			/// </summary>
			public RequestType Request { get { return m_request; } }

			/// <summary>
			/// The source of the request.
			/// </summary>
			public IMatchmakingEntity<OfferType, RequestType> Source { get { return m_source; } }

			#endregion

			//#################### CONSTRUCTORS ####################
			#region

			/// <summary>
			/// Constructs a new sourced request.
			/// </summary>
			/// <param name="request">The request.</param>
			/// <param name="source">The source of the request.</param>
			public SourcedRequest(RequestType request, IMatchmakingEntity<OfferType, RequestType> source)
			{
				m_request = request;
				m_source = source;
			}

			#endregion
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A list of outstanding offers to supply resources.
		/// </summary>
		private readonly IList<SourcedOffer> m_sourcedOffers = new List<SourcedOffer>();

		/// <summary>
		/// A list of outstanding requests to consume resources.
		/// </summary>
		private readonly IList<SourcedRequest> m_sourcedRequests = new List<SourcedRequest>();

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
			var graph = new MatchmakingGraph(m_sourcedOffers.Count, m_sourcedRequests.Count);
			for(int source = 0; source < m_sourcedOffers.Count; ++source)
			{
				OfferType offer = m_sourcedOffers[source].Offer;
				for(int destination = 0; destination < m_sourcedRequests.Count; ++destination)
				{
					RequestType request = m_sourcedRequests[destination].Request;
					int weight = request.QuantifyOffer(offer);
					if(weight > 0)
					{
						graph.AddEdge(source, destination, weight);
					}
				}
			}

			// Run the graph matching algorithm to find an optimal matching.
			graph.FindBestMatching();

			// Inform the relevant parties that they have been successfully matched.
			foreach(MatchmakingEdge edge in graph.MatchingEdges)
			{
				SourcedOffer sourcedOffer = m_sourcedOffers[edge.Source];
				SourcedRequest sourcedRequest = m_sourcedRequests[edge.Destination];
				sourcedOffer.Source.PostRequest(sourcedRequest.Request, sourcedRequest.Source);
				sourcedRequest.Source.PostOffer(sourcedOffer.Offer, sourcedOffer.Source);
			}

			// Clear all unresolved offers and requests - they will have to be resubmitted to the matchmaker next time.
			Reset();
		}

		/// <summary>
		/// Adds an offer of resources to the matchmaker.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		public void PostOffer(OfferType offer, IMatchmakingEntity<OfferType, RequestType> source)
		{
			m_sourcedOffers.Add(new SourcedOffer(offer, source));
		}

		/// <summary>
		/// Adds a request for resources to the matchmaker.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void PostRequest(RequestType request, IMatchmakingEntity<OfferType, RequestType> source)
		{
			m_sourcedRequests.Add(new SourcedRequest(request, source));
		}

		/// <summary>
		/// Resets the matchmaker (clears all existing offers and requests).
		/// </summary>
		public void Reset()
		{
			m_sourcedOffers.Clear();
			m_sourcedRequests.Clear();
		}

		#endregion
	}
}
