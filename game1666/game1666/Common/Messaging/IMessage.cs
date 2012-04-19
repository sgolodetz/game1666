/***
 * game1666: IMessage.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

namespace game1666.Common.Messaging
{
	/// <summary>
	/// An instance of a class implementing this interface represents a game message.
	/// </summary>
	interface IMessage
	{
		/// <summary>
		/// The source of the message, or null if the message has no source.
		/// </summary>
		dynamic Source { get; }
	}
}
