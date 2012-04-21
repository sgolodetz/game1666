﻿/***
 * game1666: EntityDestructionMessage.cs
 * Copyright Stuart Golodetz 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Lifetime
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
		public EntityDestructionMessage(IModelEntity source)
		{
			Source = source;
		}

		#endregion
	}
}
