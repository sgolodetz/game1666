/***
 * game1666Test: EntityTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Entities.Components;
using game1666.Common.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test
{
	/// <summary>
	/// Tests for the Entity class.
	/// </summary>
	[TestClass]
	public sealed class EntityTest
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

		/// <summary>
		/// Test the constructor.
		/// </summary>
		[TestMethod]
		public void ConstructorTest()
		{
			// Register special XML elements with the object persister.
			ObjectPersister.RegisterSpecialElement("Entity", typeof(Entity));
			ObjectPersister.RegisterSpecialElement("TestComponent", typeof(TestComponent));

			// Load the world from XML.
			var world = new Entity(XElement.Parse(@"
			<Entity>
				<property name=""Archetype"" type=""string"" value=""World""/>
				<property name=""Name"" type=""string"" value="".""/>
				<TestComponent/>
				<Entity>
					<property name=""Archetype"" type=""string"" value=""Settlement""/>
					<property name=""Name"" type=""string"" value=""settlement:Stuartopolis""/>
					<TestComponent/>
				</Entity>
			</Entity>
			"));

			// Check that it was loaded correctly.
			Assert.Equal("World", world.Archetype);
			Assert.Equal(".", world.Name);
			Assert.NotNull(world.GetComponent<TestComponent>(TestComponent.StaticGroup));

			IEntity settlement = world.GetChild("settlement:Stuartopolis");

			Assert.NotNull(settlement);
			Assert.Equal("Settlement", settlement.Archetype);
			Assert.Equal("settlement:Stuartopolis", settlement.Name);
			Assert.NotNull(settlement.GetComponent<TestComponent>(TestComponent.StaticGroup));
		}

		/// <summary>
		/// Test the GetAbsolutePath method.
		/// </summary>
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

		/// <summary>
		/// Test the GetComponent method.
		/// </summary>
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

		/// <summary>
		/// Test the SaveToXML method.
		/// </summary>
		[TestMethod]
		public void SaveToXMLTest()
		{
			// Register special XML elements with the object persister.
			ObjectPersister.RegisterSpecialElement("Entity", typeof(Entity));
			ObjectPersister.RegisterSpecialElement("TestComponent", typeof(TestComponent));

			// Construct the world.
			var world = new Entity(".", "World");
			var settlementA = new Entity("settlement:A", "Settlement");
			var settlementB = new Entity("settlement:B", "Settlement");
			var house = new Entity("house:Wibble", "House");

			world.AddChild(settlementA);
			world.AddChild(settlementB);
			settlementB.AddChild(house);

			new TestComponent(new Dictionary<string,dynamic> { { "Alue", 42 } }).AddToEntity(world);
			new TestComponent().AddToEntity(house);

			// Save the world to XML and compare it to the expected result.
			Assert.Equal(XElement.Parse(@"
			<Entity>
				<property name=""Archetype"" type=""string"" value=""World""/>
				<property name=""Name"" type=""string"" value="".""/>
				<TestComponent>
					<property name=""Alue"" type=""int"" value=""42""/>
				</TestComponent>
				<Entity>
					<property name=""Archetype"" type=""string"" value=""Settlement""/>
					<property name=""Name"" type=""string"" value=""settlement:A""/>
				</Entity>
				<Entity>
					<property name=""Archetype"" type=""string"" value=""Settlement""/>
					<property name=""Name"" type=""string"" value=""settlement:B""/>
					<Entity>
						<property name=""Archetype"" type=""string"" value=""House""/>
						<property name=""Name"" type=""string"" value=""house:Wibble""/>
						<TestComponent/>
					</Entity>
				</Entity>
			</Entity>").ToString(), world.SaveToXML().ToString());
		}

		#endregion
	}
}
