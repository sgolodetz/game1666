/***
 * game1666: IMatchmakingParticipant.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

namespace game1666.Common.Matchmaking
{
	/// <summary>
	/// An instance of this class represents an object that can participate in matchmaking.
	/// </summary>
	/// <typeparam name="OfferType">The type of matchmaking offer being used.</typeparam>
	/// <typeparam name="RequestType">The type of matchmaking request being used.</typeparam>
	public interface IMatchmakingParticipant<OfferType, RequestType>
	{
		/// <summary>
		/// Informs the object of a confirmed matchmaking offer.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		void ConfirmMatchmakingOffer(OfferType offer, IMatchmakingParticipant<OfferType, RequestType> source);

		/// <summary>
		/// Informs the object of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		void ConfirmMatchmakingRequest(RequestType request, IMatchmakingParticipant<OfferType, RequestType> source);
	}
}
