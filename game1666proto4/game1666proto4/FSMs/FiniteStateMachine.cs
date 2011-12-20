/***
 * game1666proto4: FiniteStateMachine.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a finite state machine.
	/// </summary>
	/// <typeparam name="StateID">The type used to identify the different states in the machine.</typeparam>
	sealed class FiniteStateMachine<StateID>
	{
		//#################### DELEGATES ####################
		#region

		private delegate StateID Transition(dynamic fromState);
		public delegate StateID Transition<State>(State fromState);

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The ID of the current state.
		/// </summary>
		private StateID m_currentStateID;

		/// <summary>
		/// A lookup table of all the states in the machine.
		/// </summary>
		private IDictionary<StateID,dynamic> m_states = new Dictionary<StateID,dynamic>();

		/// <summary>
		/// A lookup table of all the transitions leading out of different states in the machine.
		/// </summary>
		private IDictionary<StateID,List<Transition>> m_transitions = new Dictionary<StateID,List<Transition>>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The current state.
		/// </summary>
		public dynamic CurrentState { get { return m_states[m_currentStateID]; } }

		/// <summary>
		/// The ID of the current state.
		/// </summary>
		public StateID CurrentStateID { get { return m_currentStateID; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new finite state machine with the specified states and initial state.
		/// </summary>
		/// <param name="states">The states in the finite state machine.</param>
		/// <param name="initialStateID">The initial state.</param>
		public FiniteStateMachine(IDictionary<StateID,IFSMState<StateID>> states, StateID initialStateID)
		{
			// Copy the specified states into our local lookup table - done this way to convert the types to dynamic.
			foreach(var kv in states)
			{
				m_states.Add(kv.Key, kv.Value);
			}

			m_currentStateID = initialStateID;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a new transition out of the specified state.
		/// </summary>
		/// <param name="fromStateID">The starting state for the transition.</param>
		/// <param name="transition">The transition.</param>
		public void AddTransition<State>(StateID fromStateID, Transition<State> transition)
			where State : IFSMState<StateID>
		{
			// Forward to the private method that accepts a delegate taking a dynamic 'from state' parameter.
			AddTransition(fromStateID, s => transition(s));
		}

		/// <summary>
		/// Updates the finite state machine based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// Update the current state.
			dynamic currentState = CurrentState;	// note: we cache the current state to avoid repeating the lookup
			currentState.Update(gameTime);

			// Process any transitions.
			List<Transition> relevantTransitions;
			m_transitions.TryGetValue(m_currentStateID, out relevantTransitions);
			if(relevantTransitions == null) return;

			foreach(Transition transition in relevantTransitions)
			{
				StateID toStateID = transition(currentState);
				if(!EqualityComparer<StateID>.Default.Equals(toStateID, m_currentStateID))
				{
					m_currentStateID = toStateID;
					break;
				}
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Adds a new transition out of the specified state.
		/// </summary>
		/// <param name="fromStateID">The starting state for the transition.</param>
		/// <param name="transition">The transition.</param>
		private void AddTransition(StateID fromStateID, Transition transition)
		{
			// Look up the existing list of transitions from the specified state, creating it if it doesn't exist.
			List<Transition> relevantTransitions;
			m_transitions.TryGetValue(fromStateID, out relevantTransitions);
			if(relevantTransitions == null)
			{
				relevantTransitions = new List<Transition>();
				m_transitions[fromStateID] = relevantTransitions;
			}

			// Add the new transition to the list.
			relevantTransitions.Add(transition);
		}

		#endregion
	}
}
