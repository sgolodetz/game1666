/***
 * game1666proto4: EntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
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
}
