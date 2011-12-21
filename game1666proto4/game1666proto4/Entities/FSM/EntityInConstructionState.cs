/***
 * game1666proto4: EntityInConstructionState.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
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
}
