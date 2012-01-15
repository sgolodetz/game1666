/***
 * game1666proto4: MessageSystem.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666proto4.Common.Communication
{
	/// <summary>
	/// An instance of this class represents a message system, used to dispatch messages across the game.
	/// </summary>
	sealed class MessageSystem
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A set of rules that control how messages are dispatched.
		/// </summary>
		private ISet<MessageRule<dynamic>> m_rules = new HashSet<MessageRule<dynamic>>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Posts a new message to any interested parties. (Interest is determined using the dispatch rules.)
		/// </summary>
		/// <param name="message">The message.</param>
		public void PostMessage(IMessage message)
		{
			foreach(MessageRule<dynamic> rule in m_rules)
			{
				if(rule.Filter(message))
				{
					rule.Action(message);
				}
			}
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="filter">A filter that determines whether or not a given message is interesting.</param>
		/// <param name="action">An action to run on the message if it is interesting.</param>
		/// <returns>The internal representation of the rule, so that it can be unregistered later if desired.</returns>
		public MessageRule<dynamic> RegisterRule<T>(Func<IMessage,bool> filter, Action<T> action)
		{
			MessageRule<dynamic> rule = new MessageRule<dynamic>
			{
				Action = msg => action(msg),
				Filter = msg => filter(msg)
			};
			m_rules.Add(rule);
			return rule;
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="rule">The rule to register.</param>
		/// <returns>The internal representation of the rule, so that it can be unregistered later if desired.</returns>
		public MessageRule<dynamic> RegisterRule<T>(MessageRule<T> rule)
		{
			return RegisterRule(rule.Filter, rule.Action);
		}

		/// <summary>
		/// Unregisters a message dispatch rule.
		/// </summary>
		/// <param name="rule">The rule to unregister.</param>
		public void UnregisterRule(MessageRule<dynamic> rule)
		{
			m_rules.Remove(rule);
		}

		#endregion
	}
}
