﻿/***
 * game1666Test: EntityTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components;
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

		private abstract class TestGroupAComponent : ModelEntityComponent
		{
			public override string Group { get { return "TestGroupA"; } }
		}

		private abstract class TestGroupBComponent : ModelEntityComponent
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
			ObjectPersister.RegisterSpecialElement("cmpTest", typeof(TestComponent));
			ObjectPersister.RegisterSpecialElement("entity", typeof(ModelEntity));

			// Load the world from XML.
			XElement worldElt = XElement.Parse(@"
			<entity>
				<property name=""Name"" type=""string"" value="".""/>
				<property name=""Prototype"" type=""string"" value=""World""/>
				<cmpTest/>
				<entity>
					<property name=""Name"" type=""string"" value=""settlement:Stuartopolis""/>
					<property name=""Prototype"" type=""string"" value=""Village""/>
					<cmpTest/>
				</entity>
			</entity>
			");
			var world = new ModelEntity(worldElt).AddDescendantsFromXML(worldElt);

			// Check that it was loaded correctly.
			Assert.Equal(".", world.Name);
			Assert.Equal("World", world.Prototype);
			Assert.NotNull(world.GetComponent<TestComponent>(ModelEntityComponentGroups.TEST));

			ModelEntity settlement = world.GetChild("settlement:Stuartopolis");

			Assert.NotNull(settlement);
			Assert.Equal("settlement:Stuartopolis", settlement.Name);
			Assert.Equal("Village", settlement.Prototype);
			Assert.NotNull(settlement.GetComponent<TestComponent>(ModelEntityComponentGroups.TEST));
		}

		/// <summary>
		/// Test the GetAbsolutePath method.
		/// </summary>
		[TestMethod]
		public void GetAbsolutePathTest()
		{
			var world = new ModelEntity(".");
			var settlement = new ModelEntity("settlement:Stuartopolis");
			var house = new ModelEntity("house:Wibble");

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
			var entity = new ModelEntity(".");
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
			ObjectPersister.RegisterSpecialElement("cmpTest", typeof(TestComponent));
			ObjectPersister.RegisterSpecialElement("entity", typeof(ModelEntity));

			// Construct the world.
			var world = new ModelEntity(".");
			var settlementA = new ModelEntity("settlement:A");
			var settlementB = new ModelEntity("settlement:B");
			var house = new ModelEntity("house:Wibble");

			world.AddChild(settlementA);
			world.AddChild(settlementB);
			settlementB.AddChild(house);

			new TestComponent(new Dictionary<string,dynamic> { { "Alue", 42 } }).AddToEntity(world);
			new TestComponent().AddToEntity(house);

			// Save the world to XML and compare it to the expected result.
			Assert.Equal(XElement.Parse(@"
			<entity>
				<property name=""Name"" type=""string"" value="".""/>
				<cmpTest>
					<property name=""Alue"" type=""int"" value=""42""/>
				</cmpTest>
				<entity>
					<property name=""Name"" type=""string"" value=""settlement:A""/>
				</entity>
				<entity>
					<property name=""Name"" type=""string"" value=""settlement:B""/>
					<entity>
						<property name=""Name"" type=""string"" value=""house:Wibble""/>
						<cmpTest/>
					</entity>
				</entity>
			</entity>").ToString(), world.SaveToXML().ToString());
		}

		#endregion
	}
}
