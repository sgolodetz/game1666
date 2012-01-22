/***
 * game1666proto4: GameMatchmakingOffer.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Matchmaking;

namespace game1666proto4.GameModel.Matchmaking
{
	/// <summary>
	/// An instance of this class is used to offer resources via the matchmaker.
	/// </summary>
	public sealed class GameMatchmakingOffer : IMatchmakingOffer
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The quantity of the resource that can be supplied.
		/// </summary>
		public int AvailableQuantity { get; set; }

		/// <summary>
		/// The type of resource offered.
		/// </summary>
		public GameResource Resource { get; set; }

		/// <summary>
		/// The source of the offer.
		/// </summary>
		public dynamic Source { get; set; }

		#endregion
	}
}
