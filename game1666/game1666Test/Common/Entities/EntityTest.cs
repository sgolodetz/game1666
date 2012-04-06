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

		private abstract class TestGroupAComponent : EntityComponent
		{
			public override string Group { get { return "TestGroupA"; } }
		}

		private abstract class TestGroupBComponent : EntityComponent
		{
			public override string Group { get { return "TestGroupB"; } }
		}

		private sealed class Test1Component : TestGroupAComponent
		{
			public override string Name	{ get { return "Test1"; } }

			public string Test2SiblingName()
			{
				return Entity.GetComponent<TestGroupBComponent>("TestGroupB").Name;
			}
		}

		private sealed class Test2Component : TestGroupBComponent
		{
			public override string Name	{ get { return "Test2"; } }
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
			var component1 = new Test1Component();
			var component2 = new Test2Component();

			component1.AddToEntity(entity);
			component2.AddToEntity(entity);

			Assert.Equal(component1, entity.GetComponent<TestGroupAComponent>("TestGroupA"));
			Assert.Equal(entity, component1.Entity);
			Assert.Equal(component2.Name, component1.Test2SiblingName());
		}

		#endregion
	}
}
