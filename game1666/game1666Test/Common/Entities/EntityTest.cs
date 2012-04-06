using System.Collections.Generic;
using game1666.Common.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test
{
	[TestClass]
	public class EntityTest
	{
		//#################### HELPER CLASSES ####################
		#region

		private abstract class TestingComponent : IEntityComponent
		{
			public string Group			{ get { return "Testing"; } }
			public abstract string Name	{ get; }
		}

		private sealed class TestComponent : TestingComponent
		{
			public override string Name	{ get { return "Test"; } }
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void GetAbsolutePathTest()
		{
			var world = new Entity(".", "World");
			var settlement = new Entity("settlement:Stuartopolis", "Settlement");
			var house = new Entity("house:Wibble", "House");

			world.AddChild(settlement);
			settlement.AddChild(house);

			Assert.Equal(house.GetAbsolutePath(), "./settlement:Stuartopolis/house:Wibble");
		}

		[TestMethod]
		public void GetComponentTest()
		{
			var entity = new Entity(".", "");
			var component = new TestComponent();
			entity.AddComponent(component);
			Assert.Equal(component, entity.GetComponent<TestingComponent>("Testing"));
		}

		#endregion
	}
}
