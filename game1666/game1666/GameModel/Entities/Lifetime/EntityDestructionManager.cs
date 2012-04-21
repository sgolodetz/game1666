/***
 * game1666: EntityDestructionManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.ADTs;
using game1666.Common.Messaging;
using game1666.Common.Util;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Lifetime
{
	/// <summary>
	/// An instance of this class manages an entity destruction queue that can be used to delete entities in a robust way.
	/// </summary>
	sealed class EntityDestructionManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The priority queue of entities waiting for destruction.
		/// </summary>
		private readonly PriorityQueue<IModelEntity,float,bool> m_destructionQueue = new PriorityQueue<IModelEntity,float,bool>(new GreaterComparer<float>());

		/// <summary>
		/// The message system used to alert other entities to destruction events.
		/// </summary>
		private readonly MessageSystem m_messageSystem;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Creates a new entity destruction manager.
		/// </summary>
		/// <param name="messageSystem">The message system used to alert other entities to destruction events.</param>
		public EntityDestructionManager(MessageSystem messageSystem)
		{
			m_messageSystem = messageSystem;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Destructs any entities currently waiting on the queue.
		/// </summary>
		public void FlushQueue()
		{
			while(!m_destructionQueue.Empty)
			{
				var e = m_destructionQueue.Top;

				if(e.Data)
				{
					// The entity pre-destruction message has already been sent, so remove the destructed entity
					// from the queue and send the entity destruction message.
					m_destructionQueue.Pop();
					m_messageSystem.DispatchMessage(new EntityDestructionMessage(e.ID));

					// Unregister any message rules associated with the entity that just got destructed.
					m_messageSystem.UnregisterRulesMentioning(e.ID);
				}
				else
				{
					// The entity pre-destruction message has not yet been sent, so send it.
					e.Data = true;
					m_messageSystem.DispatchMessage(new EntityPreDestructionMessage(e.ID, e.Key));
				}
			}
		}

		/// <summary>
		/// Queues the specified entity for destruction, with the specified priority.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="priority">Its destruction priority.</param>
		public void QueueForDestruction(IModelEntity entity, float priority = 1f)
		{
			m_destructionQueue.Insert(entity, priority, false);
		}

		#endregion
	}
}
