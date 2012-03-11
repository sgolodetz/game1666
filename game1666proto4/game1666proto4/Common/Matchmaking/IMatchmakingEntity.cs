/***
 * game1666proto4: IMatchmakingEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// An instance of this class represents an entity that can partake in matchmaking.
	/// </summary>
	/// <typeparam name="OfferType">The type of matchmaking offer being used.</typeparam>
	/// <typeparam name="RequestType">The type of matchmaking request being used.</typeparam>
	public interface IMatchmakingEntity<OfferType, RequestType>
	{
		/// <summary>
		/// Informs the entity of a confirmed matchmaking offer.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		void ConfirmMatchmakingOffer(OfferType offer, IMatchmakingEntity<OfferType, RequestType> source);

		/// <summary>
		/// Informs the entity of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		void ConfirmMatchmakingRequest(RequestType request, IMatchmakingEntity<OfferType, RequestType> source);
	}
}
