/***
 * game1666proto4: IMatchmakingOffer.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Matchmaking
{
	/// <summary>
	/// An instance of a class implementing this interface is used to offer resources via the matchmaker.
	/// </summary>
	interface IMatchmakingOffer
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The source of the offer.
		/// </summary>
		dynamic Source { get; }

		#endregion
	}
}
