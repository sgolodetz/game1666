/***
 * game1666: EntityPreDestructionMessage.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;

namespace game1666.GameModel.Entities.Messages
{
	/// <summary>
	/// An instance of this class represents a message indicating that a particular game entity will shortly be destructed.
	/// It is sent to allow any entities that should be destructed first to queue themselves for destruction.
	/// </summary>
	sealed class EntityPreDestructionMessage : IMessage
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The priority of the entity on the destruction queue.
		/// </summary>
		public float Priority { get; private set; }

		/// <summary>
		/// The entity that will shortly be destructed.
		/// </summary>
		public dynamic Source { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity pre-destruction message.
		/// </summary>
		/// <param name="source">The entity that will shortly be destructed.</param>
		/// <param name="priority">The priority of the entity on the destruction queue.</param>
		public EntityPreDestructionMessage(dynamic source, float priority)
		{
			Source = source;
			Priority = priority;
		}

		#endregion
	}
}
