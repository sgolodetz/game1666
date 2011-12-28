/***
 * game1666proto4: EntityInConstructionState.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.FSMs;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.FSMs
{
	/// <summary>
	/// A state representing a time in which the entity is being constructed.
	/// </summary>
	sealed class EntityInConstructionState : IFSMState<EntityStateID>
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
		private int m_timeToConstruct;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Supplies the properties of the entity whose state is managed by the containing FSM.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties
		{
			set
			{
				Blueprint blueprint = SceneGraph.GetEntityByPath("blueprints/" + value["Blueprint"]);
				m_timeToConstruct = blueprint.TimeToConstruct;
			}
		}

		/// <summary>
		/// The ID of the state.
		/// </summary>
		public EntityStateID ID { get { return EntityStateID.IN_CONSTRUCTION; } }

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
		/// <param name="timeElapsed">The time (in milliseconds) that has elapsed since construction started.</param>
		public EntityInConstructionState(int timeElapsed)
		{
			m_timeElapsed = timeElapsed;
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
