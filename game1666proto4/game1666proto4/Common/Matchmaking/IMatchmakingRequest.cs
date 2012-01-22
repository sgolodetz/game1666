/***
 * game1666proto4: IMatchmakingRequest.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// An instance of a class implementing this interface is used to request resources via the matchmaker.
	/// </summary>
	/// <typeparam name="OfferType">The type of matchmaking offer that corresponds to the class implementing this interface.</typeparam>
	interface IMatchmakingRequest<OfferType>
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Quantifies the quality of the specified offer (i.e. the extent to which it satisfies the request).
		/// </summary>
		/// <param name="offer">The offer to quantify.</param>
		/// <returns>A number indicating the quality of the offer, on a scale from 0 (useless) to 10 (bite their arm off).</returns>
		int QuantifyOffer(OfferType offer);

		#endregion
	}
}
