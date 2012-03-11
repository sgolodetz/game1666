/***
 * game1666proto4: MessageSystem.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;

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
		/// A dictionary of rules that control how messages are dispatched.
		/// </summary>
		private static readonly IDictionary<string,MessageRule<dynamic>> s_rules = new Dictionary<string,MessageRule<dynamic>>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Instantly dispatches a message to any interested parties.
		/// </summary>
		/// <param name="message">The message.</param>
		public static void DispatchMessage(IMessage message)
		{
			// Make a list of all the rules that need executing for this message. We do this before trying
			// to execute any rules, because rules are allowed to unregister themselves and we want to avoid
			// modifying the rule collection while we're iterating over it.
			var rulesToExecute = new List<MessageRule<dynamic>>();
			foreach(MessageRule<dynamic> rule in s_rules.Values)
			{
				if(rule.Filter(message))
				{
					rulesToExecute.Add(rule);
				}
			}

			// Execute each of the rules on the message in turn.
			foreach(MessageRule<dynamic> rule in rulesToExecute)
			{
				rule.Action(message);
			}
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="key">A unique key that can be used to refer to the message rule.</param>
		/// <param name="filter">A filter that determines whether or not a given message is interesting.</param>
		/// <param name="action">An action to run on the message if it is interesting.</param>
		/// <param name="entities">The entities mentioned by this rule (can be null if there aren't any).</param>
		public static void RegisterRule<T>(string key, Func<IMessage,bool> filter, Action<T> action, List<dynamic> entities)
		{
			var rule = new MessageRule<dynamic>
			{
				Action = msg => action(msg),
				Entities = entities,
				Filter = msg => filter(msg),
				Key = key
			};
			s_rules.Add(rule.Key, rule);
		}

		/// <summary>
		/// Registers a new message dispatch rule.
		/// </summary>
		/// <typeparam name="T">The type of message with which the rule deals.</typeparam>
		/// <param name="rule">The rule to register.</param>
		public static void RegisterRule<T>(MessageRule<T> rule)
		{
			RegisterRule(rule.Key, rule.Filter, rule.Action, rule.Entities);
		}

		/// <summary>
		/// Unregisters all the message dispatch rules.
		/// </summary>
		public static void UnregisterAllRules()
		{
			s_rules.Clear();
		}

		/// <summary>
		/// Unregisters a message dispatch rule.
		/// </summary>
		/// <param name="key">The key of the rule to unregister.</param>
		public static void UnregisterRule(string key)
		{
			s_rules.Remove(key);
		}

		/// <summary>
		/// Unregisters all the message dispatch rules that mention the specified entity.
		/// </summary>
		/// <param name="entity">The entity whose rules we want to unregister.</param>
		public static void UnregisterRulesMentioning(dynamic entity)
		{
			// Note: The .ToList() call here is deliberate - we can't filter and modify s_rules simultaneously.
			foreach(var rule in s_rules.Values.Where(r => r.Entities != null && r.Entities.Contains(entity)).ToList())
			{
				s_rules.Remove(rule.Key);
			}
		}

		#endregion
	}
}
