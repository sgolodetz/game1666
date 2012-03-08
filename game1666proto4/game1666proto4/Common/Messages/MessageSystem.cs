/***
 * game1666proto4: MessageSystem.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666proto4.Common.Messages
{
	/// <summary>
	/// This class implements a message system, used to dispatch messages across the game.
	/// </summary>
	static class MessageSystem
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A queue of messages to be dispatched on request.
		/// </summary>
		private static readonly Queue<IMessage> s_messageQueue = new Queue<IMessage>();

		/// <summary>
		/// A set of rules that control how messages are dispatched.
		/// </summary>
		private static readonly ISet<MessageRule<dynamic>> s_rules = new HashSet<MessageRule<dynamic>>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a new message to the queue.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void PostMessage(IMessage message)
		{
			s_messageQueue.Enqueue(message);
		}

		/// <summary>
		/// Processes and clears the message queue, alerting any interested parties for each message.
		/// (Interest is determined using the dispatch rules).
		/// </summary>
		public static void ProcessMessageQueue()
		{
			foreach(IMessage message in s_messageQueue)
			{
				foreach(MessageRule<dynamic> rule in s_rules)
				{
					if(rule.Filter(message))
					{
						rule.Action(message);
					}
				}
			}
			s_messageQueue.Clear();
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="filter">A filter that determines whether or not a given message is interesting.</param>
		/// <param name="action">An action to run on the message if it is interesting.</param>
		/// <returns>The internal representation of the rule, so that it can be unregistered later if desired.</returns>
		public static MessageRule<dynamic> RegisterRule<T>(Func<IMessage,bool> filter, Action<T> action)
		{
			var rule = new MessageRule<dynamic>
			{
				Action = msg => action(msg),
				Filter = msg => filter(msg)
			};
			s_rules.Add(rule);
			return rule;
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="rule">The rule to register.</param>
		/// <returns>The internal representation of the rule, so that it can be unregistered later if desired.</returns>
		public static MessageRule<dynamic> RegisterRule<T>(MessageRule<T> rule)
		{
			return RegisterRule(rule.Filter, rule.Action);
		}

		/// <summary>
		/// Unregisters a message dispatch rule.
		/// </summary>
		/// <param name="rule">The rule to unregister.</param>
		public static void UnregisterRule(MessageRule<dynamic> rule)
		{
			s_rules.Remove(rule);
		}

		#endregion
	}
}
