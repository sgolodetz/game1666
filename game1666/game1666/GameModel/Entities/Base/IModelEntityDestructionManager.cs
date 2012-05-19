/***
 * game1666: IModelEntityDestructionManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;

namespace game1666.GameModel.Entities.Base
{
	/// <summary>
	/// An instance of a class implementing this interface manages an entity destruction
	/// queue that can be used to delete model entities in a robust way.
	/// </summary>
	interface IModelEntityDestructionManager
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The message system used to alert other entities to destruction events.
		/// </summary>
		MessageSystem MessageSystem { get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Destructs any entities currently waiting on the queue.
		/// </summary>
		void FlushQueue();

		/// <summary>
		/// Queues the specified entity for destruction, with the specified priority.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="priority">Its destruction priority.</param>
		void QueueForDestruction(ModelEntity entity, float priority = 1f);

		#endregion
	}
}
