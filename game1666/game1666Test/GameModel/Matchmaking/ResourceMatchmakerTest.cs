/***
 * game1666Test: ResourceMatchmakerTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Matchmaking;
using game1666.GameModel.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test.GameModel.Matchmaking
{
	[TestClass]
	public sealed class ResourceMatchmakerTest
	{
		//#################### HELPER CLASSES ####################
		#region

		public sealed class House : IMatchmakingParticipant<ResourceOffer, ResourceRequest>
		{
			public IMatchmakingParticipant<ResourceOffer, ResourceRequest> OfferSource { get; private set; }

			public void ConfirmMatchmakingOffer(ResourceOffer offer, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
			{
				OfferSource = source;
			}

			public void ConfirmMatchmakingRequest(ResourceRequest request, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
			{}
		}

		public sealed class Walker : IMatchmakingParticipant<ResourceOffer, ResourceRequest>
		{
			public IMatchmakingParticipant<ResourceOffer, ResourceRequest> RequestSource { get; private set; }

			public void ConfirmMatchmakingOffer(ResourceOffer offer, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
			{}

			public void ConfirmMatchmakingRequest(ResourceRequest request, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
			{
				RequestSource = source;
			}
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void MatchTest()
		{
			var house = new House();
			var walker = new Walker();

			var matchmaker = new ResourceMatchmaker();
			matchmaker.PostRequest
			(
				new ResourceRequest
				{
					Resource = Resource.OCCUPANCY,
					DesiredQuantity = 2,
					MinimumQuantity = 1
				},
				house
			);
			matchmaker.PostOffer
			(
				new ResourceOffer
				{
					Resource = Resource.OCCUPANCY,
					AvailableQuantity = 1
				},
				walker
			);
			matchmaker.Match();

			Assert.Equal(walker.RequestSource, house);
			Assert.Equal(house.OfferSource, walker);
		}

		#endregion
	}
}
