/***
 * game1666proto4: MessageRuleFactory.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;

namespace game1666proto4.Common.Communication
{
	/// <summary>
	/// This class provides methods to create commonly-used types of message rule.
	/// </summary>
	static class MessageRuleFactory
	{
		/// <summary>
		/// Constructs a message rule that filters for messages of a given type from the specified source.
		/// </summary>
		/// <typeparam name="T">The type of message for which to filter.</typeparam>
		/// <param name="source">The source in which we're interested.</param>
		/// <param name="action">The action to take when the source posts a message.</param>
		/// <returns>The message rule.</returns>
		public static MessageRule<T> FromSource<T>(dynamic source, Action<T> action)
		{
			return new MessageRule<T>
			{
				Action = action,
				Filter = msg => msg.GetType() == typeof(T) && msg.Source.GetType() == source.GetType() && msg.Source == source
			};
		}
	}
}
