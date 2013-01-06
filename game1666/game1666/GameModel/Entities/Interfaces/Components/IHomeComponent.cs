/***
 * game1666: IHomeComponent.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.Matchmaking;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Matchmaking;

namespace game1666.GameModel.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface allows its containing entity to act as a home for entities.
	/// </summary>
	interface IHomeComponent : IEntityComponent, IMatchmakingParticipant<ResourceOffer,ResourceRequest>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The number of people who currently live in this home.
		/// </summary>
		int CurrentOccupants { get; }

		/// <summary>
		/// The entity containing the component (if any).
		/// </summary>
		ModelEntity Entity { get; }

		#endregion
	}
}
