using System.Collections.Generic;
using game1666.Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test
{
	[TestClass]
	public class EntityTest
	{
		[TestMethod]
		public void GetAbsolutePathTest()
		{
			var world = new Entity(new Dictionary<string,dynamic>
			{
				{ "Archetype", "World" },
				{ "Name", "." }
			});

			var settlement = new Entity(new Dictionary<string,dynamic>
			{
				{ "Archetype", "Settlement" },
				{ "Name", "settlement:Stuartopolis" }
			});

			var house = new Entity(new Dictionary<string,dynamic>
			{
				{ "Archetype", "House" },
				{ "Name", "house:Wibble" }
			});

			world.AddChild(settlement);
			settlement.AddChild(house);

			Assert.Equal(house.GetAbsolutePath(), "./settlement:Stuartopolis/house:Wibble");
		}
	}
}
