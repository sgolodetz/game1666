/***
 * game1666proto4: ResourceRequest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Matchmaking;

namespace game1666proto4.GameModel.Matchmaking
{
	/// <summary>
	/// An instance of this class is used to request resources via the matchmaker.
	/// </summary>
	public sealed class ResourceRequest : IMatchmakingRequest<ResourceOffer>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The quantity of the resource that is desired.
		/// </summary>
		public int DesiredQuantity { get; set; }

		/// <summary>
		/// The quantity of the resource that is absolutely necessary.
		/// </summary>
		public int MinimumQuantity { get; set; }

		/// <summary>
		/// The type of resource requested.
		/// </summary>
		public Resource Resource { get; set; }

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
		public int QuantifyOffer(ResourceOffer offer)
		{
			if(offer.Resource == Resource && offer.AvailableQuantity >= MinimumQuantity)
			{
				return Math.Min(10 * offer.AvailableQuantity / DesiredQuantity, 10);
			}
			else return 0;
		}

		#endregion
	}
}
