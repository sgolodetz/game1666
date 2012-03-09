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
		/// A set of entities that have been destructed and whose rules therefore need to be removed.
		/// </summary>
		private static readonly ISet<dynamic> s_destructedEntities = new HashSet<dynamic>();

		/// <summary>
		/// A queue of messages to be dispatched on request.
		/// </summary>
		private static readonly Queue<IMessage> s_messageQueue = new Queue<IMessage>();

		/// <summary>
		/// A dictionary of rules that control how messages are dispatched.
		/// </summary>
		private static readonly IDictionary<string,MessageRule<dynamic>> s_rules = new Dictionary<string,MessageRule<dynamic>>();

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
				// Make a list of all the rules that need executing for this message. We do this before trying
				// to execute any rules, because rules are allowed unregister themselves and we want to avoid
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
			s_messageQueue.Clear();

			// Remove any rules referring to destructed entities.
			foreach(dynamic entity in s_destructedEntities)
			{
				UnregisterRulesMentioning(entity);
			}
			s_destructedEntities.Clear();
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
			// Register the message rule itself.
			var rule = new MessageRule<dynamic>
			{
				Action = msg => action(msg),
				Entities = entities,
				Filter = msg => filter(msg),
				Key = key
			};
			s_rules.Add(rule.Key, rule);

			// Register local destruction rules for the entities mentioned within the rule. These are
			// used to keep the set of message rules up-to-date when entities get destroyed.
			foreach(var entity in entities)
			{
				RegisterEntityDestructionRule(entity);
			}
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
			foreach(var rule in s_rules.Values.Where(r => r.Entities != null && r.Entities.Contains(entity)).ToList())
			{
				s_rules.Remove(rule.Key);
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Registers a local destruction rule for the specified entity if one doesn't already exist.
		/// This is used to make sure that all the rules corresponding to the entity get deleted if
		/// the entity gets destructed.
		/// </summary>
		/// <param name="entity">The entity for which to register a destruction rule.</param>
		private static void RegisterEntityDestructionRule(dynamic entity)
		{
			// Ensure only one destruction rule is added per entity (extra rules would be redundant).
			if(s_rules.Values.Any(r =>
				r.Entities.Count == 1 &&
				r.Entities[0].GetType() == entity.GetType() &&
				r.Entities[0] == entity &&
				r.Key.StartsWith("ruledestructor_")))
			{
				return;
			}

			// Create and add the rule itself. Its action, which is triggered only when the entity
			// in question gets destructed, adds the entity to a set of destructed entities whose
			// rules must be removed after processing the message queue. (Note that if we removed
			// the rules before processing the message queue, some of the entities might not
			// receive the entity destruction message.)
			var rule = new MessageRule<dynamic>
			{
				Action = msg => s_destructedEntities.Add(entity),
				Entities = new List<dynamic> { entity },
				Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(entity),
				Key = "ruledestructor_" + Guid.NewGuid().ToString()
			};
			s_rules.Add(rule.Key, rule);
		}

		#endregion
	}
}
