/***
 * game1666proto4Test: EntityDestructionManagerTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666proto4.Common.Messages;
using game1666proto4.GameModel.Entities.Lifetime;
using game1666proto4.GameModel.Entities.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Entities
{
	[TestClass]
	public sealed class EntityDestructionManagerTest
	{
		//#################### HELPER CLASSES ####################
		#region

		/// <summary>
		/// An instance of this class represents a "generic" test entity that can
		/// contain other entities and have a movement target.
		/// </summary>
		private class TestEntity
		{
			//#################### PROPERTIES ####################
			#region

			public List<TestEntity> Children	{ get; private set; }
			public string Name					{ get; private set; }
			public TestEntity Target			{ get; private set; }

			#endregion

			//#################### CONSTRUCTORS ####################
			#region

			public TestEntity(string name)
			{
				Name = name;
				Children = new List<TestEntity>();

				MessageSystem.RegisterRule
				(
					new MessageRule<EntityPreDestructionMessage>
					{
						Action = msg =>
						{
							foreach(var c in Children)
							{
								EntityDestructionManager.QueueForDestruction(c, msg.Priority + 1);
							}
						},
						Entities = new List<dynamic> { this },
						Filter = MessageFilterFactory.TypedFromSource<EntityPreDestructionMessage>(this),
						Key = Guid.NewGuid().ToString()
					}
				);
			}

			#endregion

			//#################### PUBLIC METHODS ####################
			#region

			public void AddChild(TestEntity child)
			{
				Children.Add(child);

				MessageSystem.RegisterRule
				(
					new MessageRule<EntityDestructionMessage>
					{
						Action = msg => DeleteChild(child),
						Entities = new List<dynamic> { child, this },
						Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(child),
						Key = Guid.NewGuid().ToString()
					}
				);
			}

			public void SetTarget(TestEntity target)
			{
				Target = target;

				MessageSystem.RegisterRule
				(
					new MessageRule<EntityDestructionMessage>
					{
						Action = msg => ClearTarget(target),
						Entities = new List<dynamic> { target, this },
						Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(target),
						Key = Guid.NewGuid().ToString()
					}
				);
			}

			#endregion

			//#################### PRIVATE METHODS ####################
			#region

			private void ClearTarget(TestEntity target)
			{
				Target = null;
			}

			private void DeleteChild(TestEntity child)
			{
				Children.Remove(child);
			}

			#endregion
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void FlushQueueTest()
		{
			var building1 = new TestEntity("building1");
			var building2 = new TestEntity("building2");
			var cityA = new TestEntity("cityA");
			var cityB = new TestEntity("cityB");
			var world = new TestEntity("world");
			var walker = new TestEntity("walker");

			world.AddChild(cityA);
			world.AddChild(cityB);
			world.AddChild(walker);
			cityB.AddChild(building1);
			cityB.AddChild(building2);
			walker.SetTarget(building2);

				Assert.Contains(cityA, world.Children);
				Assert.Contains(cityB, world.Children);
				Assert.Contains(walker, world.Children);
				Assert.Contains(building1, cityB.Children);
				Assert.Contains(building2, cityB.Children);
				Assert.Equal(walker.Target, building2);
			
			EntityDestructionManager.QueueForDestruction(cityB, 1f);
			EntityDestructionManager.FlushQueue();

				Assert.DoesNotContain(cityB, world.Children);
				Assert.DoesNotContain(building1, cityB.Children);
				Assert.DoesNotContain(building2, cityB.Children);
				Assert.Equal(walker.Target, null);

			MessageSystem.UnregisterAllRules();
		}

		#endregion
	}
}
