/***
 * game1666proto4: EntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ MAIN CLASS @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

	/// <summary>
	/// The various possible states of an entity.
	/// </summary>
	enum EntityState
	{
		IN_CONSTRUCTION,
		OPERATING
	}

	/// <summary>
	/// An instance of this class is used to manage the state of an entity over time.
	/// </summary>
	sealed class EntityFSM : FiniteStateMachine<EntityState>
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity builder that will take the specified amount of time to finish building the entity.
		/// </summary>
		/// <param name="timeToConstruct">The overall time required to construct the entity (in milliseconds).</param>
		public EntityFSM(int timeToConstruct)
		{
			// Add the necessary states.
			AddState(EntityState.IN_CONSTRUCTION, new EntityInConstructionState(timeToConstruct));
			AddState(EntityState.OPERATING, new EntityOperatingState());

			// Add the necessary transitions.
			AddTransition(EntityState.IN_CONSTRUCTION, (EntityInConstructionState s) =>
				s.PercentComplete >= 100 ? EntityState.OPERATING : EntityState.IN_CONSTRUCTION
			);

			// Set the starting state.
			CurrentStateID = EntityState.IN_CONSTRUCTION;
		}

		#endregion
	}

	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ ENTITY STATES @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

	/// <summary>
	/// A state representing a time in which the entity is being constructed.
	/// </summary>
	sealed class EntityInConstructionState : IFSMState<EntityState>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The time (in milliseconds) that has elapsed since construction started.
		/// </summary>
		private int m_timeElapsed;

		/// <summary>
		/// The time (in milliseconds) that it takes to construct the entity.
		/// </summary>
		private readonly int m_timeToConstruct;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The ID of the state.
		/// </summary>
		public EntityState ID { get { return EntityState.IN_CONSTRUCTION; } }

		/// <summary>
		/// The completeness percentage of the entity.
		/// </summary>
		public int PercentComplete { get { return m_timeElapsed * 100 / m_timeToConstruct; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new 'entity in construction' state.
		/// </summary>
		/// <param name="timeToConstruct">The time (in milliseconds) that it takes to construct the entity.</param>
		public EntityInConstructionState(int timeToConstruct)
		{
			m_timeElapsed = 0;
			m_timeToConstruct = timeToConstruct;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the state based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			m_timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
		}

		#endregion
	}

	/// <summary>
	/// A state representing a time in which the entity is operational.
	/// </summary>
	sealed class EntityOperatingState : IFSMState<EntityState>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The ID of the state.
		/// </summary>
		public EntityState ID { get { return EntityState.OPERATING; } }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the state based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime) {}

		#endregion
	}
}
