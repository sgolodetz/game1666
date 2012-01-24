/***
 * game1666proto4: EntityDestructionMessage.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Messages;

namespace game1666proto4.GameModel.Messages
{
	/// <summary>
	/// An instance of this class represents a message indicating that a particular game entity is being destructed.
	/// </summary>
	sealed class EntityDestructionMessage : IMessage
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity being destructed.
		/// </summary>
		public dynamic Source { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity destruction message.
		/// </summary>
		/// <param name="source">The entity being destructed.</param>
		public EntityDestructionMessage(dynamic source)
		{
			Source = source;
		}

		#endregion
	}
}
