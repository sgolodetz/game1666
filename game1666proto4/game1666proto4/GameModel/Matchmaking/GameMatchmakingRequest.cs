/***
 * game1666proto4: GameMatchmakingRequest.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Matchmaking;

namespace game1666proto4.GameModel.Matchmaking
{
	/// <summary>
	/// An instance of this class is used to request resources via the matchmaker.
	/// </summary>
	sealed class GameMatchmakingRequest : IMatchmakingRequest<GameMatchmakingOffer>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The quantity of the resource that is desired.
		/// </summary>
		public int DesiredQuantity { get; set; }

		/// <summary>
		/// The type of resource requested.
		/// </summary>
		public GameMatchmakingResource Resource { get; set; }

		/// <summary>
		/// The source of the request.
		/// </summary>
		public dynamic Source { get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Quantifies the quality of the specified offer (i.e. the extent to which it satisfies the request).
		/// </summary>
		/// <param name="offer">The offer to quantify.</param>
		/// <returns>A number indicating the quality of the offer, on a scale from 0 (useless) to 10 (bite their arm off).</returns>
		public int QuantifyOffer(GameMatchmakingOffer offer)
		{
			// The simplest possible implementation: "if enough of the correct resource is being offered, take it, otherwise refuse."
			return offer.Resource == Resource && offer.AvailableQuantity >= DesiredQuantity ? 10 : 0;
		}

		#endregion
	}
}
