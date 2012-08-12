/***
 * game1666: ResourceOffer.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

namespace game1666.GameModel.Matchmaking
{
	/// <summary>
	/// An instance of this class is used to offer resources via the matchmaker.
	/// </summary>
	public sealed class ResourceOffer
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
		public Resource Resource { get; set; }

		#endregion
	}
}
