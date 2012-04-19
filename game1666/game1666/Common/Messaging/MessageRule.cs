/***
 * game1666: MessageRule.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666.Common.Messaging
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
		/// The entities mentioned by this rule (can be null if there aren't any).
		/// </summary>
		public List<dynamic> Entities { get; set; }

		/// <summary>
		/// A filter that determines whether or not a given message is interesting.
		/// </summary>
		public Func<IMessage,bool> Filter { get; set; }

		/// <summary>
		/// A unique key that can be used to refer to the rule.
		/// </summary>
		public string Key { get; set; }

		#endregion
	}
}
