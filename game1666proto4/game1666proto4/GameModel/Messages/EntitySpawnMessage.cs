/***
 * game1666proto4: EntitySpawnMessage.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Messages;

namespace game1666proto4.GameModel.Messages
{
	/// <summary>
	/// An instance of this class represents a message indicating that a new game entity is being spawned.
	/// This will be used to alert the relevant playing area that it needs to add the new entity.
	/// </summary>
	sealed class EntitySpawnMessage : IMessage
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity being spawned.
		/// </summary>
		public dynamic Entity { get; private set; }

		/// <summary>
		/// The entity doing the spawning.
		/// </summary>
		public dynamic Source { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity spawn message.
		/// </summary>
		/// <param name="source">The entity doing the spawning.</param>
		/// <param name="entity">The entity being spawned.</param>
		public EntitySpawnMessage(dynamic source, dynamic entity)
		{
			Source = source;
			Entity = entity;
		}

		#endregion
	}
}
