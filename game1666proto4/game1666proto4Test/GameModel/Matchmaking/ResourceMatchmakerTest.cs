/***
 * game1666proto4Test: ResourceMatchmakerTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.GameModel.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.GameModel.Matchmaking
{
	[TestClass]
	public sealed class ResourceMatchmakerTest
	{
		//#################### HELPER CLASSES ####################
		#region

		public sealed class House
		{
			public dynamic OfferSource { get; private set; }

			public void PostOffer(ResourceOffer offer, dynamic source)
			{
				OfferSource = source;
			}
		}

		public sealed class Walker
		{
			public dynamic RequestSource { get; private set; }

			public void PostRequest(ResourceRequest request, dynamic source)
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
