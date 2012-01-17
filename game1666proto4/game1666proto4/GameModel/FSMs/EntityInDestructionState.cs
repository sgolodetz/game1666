/***
 * game1666proto4: EntityInDestructionState.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666proto4.Common.Communication;
using game1666proto4.Common.FSMs;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Messages;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.FSMs
{
	/// <summary>
	/// A state representing a time in which the entity is being destructed.
	/// </summary>
	sealed class EntityInDestructionState : IFSMState<EntityStateID>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The amount of construction done (in comparison to the time required to construct the entity).
		/// </summary>
		private int ConstructionDone
		{
			get	{ return m_fsmProperties["ConstructionDone"]; }
			set	{ m_fsmProperties["ConstructionDone"] = Math.Max(value, 0); }
		}

		/// <summary>
		/// The properties of the entity that contains the FSM.
		/// </summary>
		private IDictionary<string,dynamic> m_entityProperties;

		/// <summary>
		/// The properties of the FSM that contains this state.
		/// </summary>
		private IDictionary<string,dynamic> m_fsmProperties;

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
			private get
			{
				return m_entityProperties;
			}

			set
			{
				m_entityProperties = value;
				Blueprint blueprint = BlueprintManager.GetBlueprint(m_entityProperties["Blueprint"]);
				m_timeToConstruct = blueprint.TimeToConstruct;
			}
		}

		/// <summary>
		/// The ID of the state.
		/// </summary>
		public EntityStateID ID { get { return EntityStateID.IN_DESTRUCTION; } }

		/// <summary>
		/// The completeness percentage of the entity.
		/// </summary>
		public int PercentComplete { get { return ConstructionDone * 100 / m_timeToConstruct; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new 'entity in destruction' state.
		/// </summary>
		/// <param name="fsmProperties">The properties of the FSM that contains this state.</param>
		public EntityInDestructionState(IDictionary<string,dynamic> fsmProperties)
		{
			m_fsmProperties = fsmProperties;
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
			ConstructionDone -= gameTime.ElapsedGameTime.Milliseconds;
			if(ConstructionDone == 0)
			{
				MessageSystem.PostMessage(new EntityDestructionMessage(EntityProperties["Self"]));
			}
		}

		#endregion
	}
}
