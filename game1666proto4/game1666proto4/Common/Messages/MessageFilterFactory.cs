/***
 * game1666proto4: MessageFilterFactory.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666proto4.Common.Messages
{
	/// <summary>
	/// This class provides methods to create commonly-used types of message filter.
	/// </summary>
	static class MessageFilterFactory
	{
		/// <summary>
		/// Constructs a message filter that filters for messages of a given type.
		/// </summary>
		/// <typeparam name="T">The type of message for which to filter.</typeparam>
		/// <returns>The filter.</returns>
		public static Func<IMessage,bool> Typed<T>()
		{
			return msg => msg.GetType() == typeof(T);
		}

		/// <summary>
		/// Constructs a message filter that filters for messages of a given type from the specified source.
		/// </summary>
		/// <typeparam name="T">The type of message for which to filter.</typeparam>
		/// <param name="source">The source in which we're interested.</param>
		/// <returns>The filter.</returns>
		public static Func<IMessage,bool> TypedFromSource<T>(dynamic source)
		{
			return msg => msg.GetType() == typeof(T) && msg.Source.GetType() == source.GetType() && msg.Source == source;
		}
	}
}
