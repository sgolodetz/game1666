/***
 * game1666proto4: FiniteStateMachine.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a finite state machine.
	/// </summary>
	/// <typeparam name="StateID">The type used to identify the different states in the machine.</typeparam>
	abstract class FiniteStateMachine<StateID> : Entity
	{
		//#################### DELEGATES ####################
		#region

		private delegate StateID Transition(dynamic fromState);
		protected delegate StateID Transition<State>(State fromState);

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A lookup table of all the states in the machine.
		/// </summary>
		private readonly IDictionary<StateID,dynamic> m_states = new Dictionary<StateID,dynamic>();

		/// <summary>
		/// A lookup table of all the transitions leading out of different states in the machine.
		/// </summary>
		private readonly IDictionary<StateID,List<Transition>> m_transitions = new Dictionary<StateID,List<Transition>>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The current state.
		/// </summary>
		public dynamic CurrentState { get { return m_states[CurrentStateID]; } }

		/// <summary>
		/// The ID of the current state.
		/// </summary>
		public StateID CurrentStateID { get; protected set; }

		/// <summary>
		/// An enumerable of the states in the machine.
		/// </summary>
		protected IEnumerable<dynamic> States { get { return m_states.Values; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a finite state machine (FSM) from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the FSM's XML representation.</param>
		public FiniteStateMachine(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

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
			m_transitions.TryGetValue(CurrentStateID, out relevantTransitions);
			if(relevantTransitions == null) return;

			foreach(Transition transition in relevantTransitions)
			{
				StateID toStateID = transition(currentState);
				if(!EqualityComparer<StateID>.Default.Equals(toStateID, CurrentStateID))
				{
					CurrentStateID = toStateID;
					break;
				}
			}
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Adds a new state.
		/// </summary>
		/// <param name="stateID">The ID of the state.</param>
		/// <param name="state">The state itself.</param>
		protected void AddState(StateID stateID, IFSMState<StateID> state)
		{
			m_states.Add(stateID, state);
		}

		/// <summary>
		/// Adds a new transition out of the specified state.
		/// </summary>
		/// <param name="fromStateID">The starting state for the transition.</param>
		/// <param name="transition">The transition.</param>
		protected void AddTransition<State>(StateID fromStateID, Transition<State> transition)
			where State : IFSMState<StateID>
		{
			// Forward to the private method that accepts a delegate taking a dynamic 'from state' parameter.
			AddTransition(fromStateID, s => transition(s));
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
