/***
 * game1666: ResourceMatchmaker.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Matchmaking;

namespace game1666.GameModel.Matchmaking
{
	/// <summary>
	/// An instance of this class represents a matchmaker for game resources. This class essentially
	/// exists to provide a "typedef" for Matchmaker[ResourceOffer, ResourceRequest].
	/// </summary>
	sealed class ResourceMatchmaker : Matchmaker<ResourceOffer, ResourceRequest>
	{}
}
