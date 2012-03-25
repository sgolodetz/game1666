/***
 * game1666proto4: EntityDestructionManager.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.ADTs;
using game1666proto4.Common.Messages;
using game1666proto4.Common.Util;
using game1666proto4.GameModel.Messages;

namespace game1666proto4.GameModel.Lifetime
{
	/// <summary>
	/// This class manages an entity destruction queue that can be used to delete entities in a robust way.
	/// </summary>
	static class EntityDestructionManager
	{
		//#################### NESTED CLASSES ####################
		#region

		/// <summary>
		/// A special class used to wrap dynamic objects into something equatable.
		/// </summary>
		private sealed class EquatableDynamic : IEquatable<EquatableDynamic>
		{
			/// <summary>
			/// The contained dynamic object.
			/// </summary>
			public dynamic Actual { get; set; }

			/// <summary>
			/// Checks whether or not this EquatableDynamic is equal to another one.
			/// </summary>
			/// <param name="rhs">The other EquatableDynamic.</param>
			/// <returns>true, if the two instances are equal, or false otherwise.</returns>
			public bool Equals(EquatableDynamic rhs)
			{
				return object.ReferenceEquals(this, rhs);
			}
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The priority queue of entities waiting for destruction.
		/// </summary>
		private static readonly PriorityQueue<EquatableDynamic,float,bool> s_destructionQueue = new PriorityQueue<EquatableDynamic,float,bool>(new GreaterComparer<float>());

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Destructs any entities currently waiting on the queue.
		/// </summary>
		public static void FlushQueue()
		{
			while(!s_destructionQueue.Empty)
			{
				var e = s_destructionQueue.Top;

				if(e.Data)
				{
					// The entity pre-destruction message has already been sent, so remove the destructed entity
					// from the queue and send the entity destruction message.
					s_destructionQueue.Pop();
					MessageSystem.DispatchMessage(new EntityDestructionMessage(e.ID.Actual));

					// Unregister any message rules associated with the entity that just got destructed.
					MessageSystem.UnregisterRulesMentioning(e.ID.Actual);
				}
				else
				{
					// The entity pre-destruction message has not yet been sent, so send it.
					e.Data = true;
					MessageSystem.DispatchMessage(new EntityPreDestructionMessage(e.ID.Actual, e.Key));
				}
			}
		}

		/// <summary>
		/// Queues the specified entity for destruction, with the specified priority.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="priority">Its destruction priority.</param>
		public static void QueueForDestruction(dynamic entity, float priority = 1f)
		{
			s_destructionQueue.Insert(new EquatableDynamic { Actual = entity }, priority, false);
		}

		#endregion
	}
}
