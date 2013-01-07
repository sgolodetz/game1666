/***
 * game1666: IPersonComponent.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.Matchmaking;
using game1666.GameModel.Matchmaking;

namespace game1666.GameModel.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides person behaviour to its containing entity.
	/// </summary>
	interface IPersonComponent : IEntityComponent, IMatchmakingParticipant<ResourceOffer,ResourceRequest>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The absolute path of the person's home (if any).
		/// </summary>
		string HomePath { get; set; }

		#endregion
	}
}
