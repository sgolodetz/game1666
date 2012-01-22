/***
 * game1666proto4Test: GameMatchmakerTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.GameModel.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.GameModel.Matchmaking
{
	[TestClass]
	public sealed class GameMatchmakerTest
	{
		//#################### HELPER CLASSES ####################
		#region

		public sealed class House
		{
			public dynamic OfferSource { get; private set; }

			public void PostOffer(GameMatchmakingOffer offer)
			{
				OfferSource = offer.Source;
			}
		}

		public sealed class Walker
		{
			public dynamic RequestSource { get; private set; }

			public void PostRequest(GameMatchmakingRequest request)
			{
				RequestSource = request.Source;
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

			var matchmaker = new GameMatchmaker();
			matchmaker.PostRequest
			(
				new GameMatchmakingRequest
				{
					Source = house,
					Resource = GameResource.OCCUPANCY,
					DesiredQuantity = 2,
					MinimumQuantity = 1
				}
			);
			matchmaker.PostOffer
			(
				new GameMatchmakingOffer
				{
					Source = walker,
					Resource = GameResource.OCCUPANCY,
					AvailableQuantity = 1
				}
			);
			matchmaker.Match();

			Assert.Equal(walker.RequestSource, house);
			Assert.Equal(house.OfferSource, walker);
		}

		#endregion
	}
}
