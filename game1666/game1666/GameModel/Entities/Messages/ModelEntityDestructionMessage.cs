﻿/***
 * game1666: ModelEntityDestructionMessage.cs
 * Copyright Stuart Golodetz 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;

namespace game1666.GameModel.Entities.Messages
{
	/// <summary>
	/// An instance of this class represents a message indicating that a particular model entity is being destructed.
	/// </summary>
	sealed class ModelEntityDestructionMessage : IMessage
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
		public ModelEntityDestructionMessage(dynamic source)
		{
			Source = source;
		}

		#endregion
	}
}
