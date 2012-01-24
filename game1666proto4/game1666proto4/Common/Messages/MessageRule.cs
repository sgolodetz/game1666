/***
 * game1666proto4: MessageRule.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;

namespace game1666proto4.Common.Messages
{
	/// <summary>
	/// An instance of this class represents a message rule.
	/// </summary>
	/// <typeparam name="T">The message type.</typeparam>
	sealed class MessageRule<T>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// An action to run on the message if it is interesting.
		/// </summary>
		public Action<T> Action { get; set; }

		/// <summary>
		/// A filter that determines whether or not a given message is interesting.
		/// </summary>
		public Func<IMessage,bool> Filter { get; set; }

		#endregion
	}
}
