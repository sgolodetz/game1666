/***
 * game1666proto4: EntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.FSMs;

namespace game1666proto4.GameModel.FSMs
{
	/// <summary>
	/// An instance of this class is used to manage the state of an entity over time.
	/// </summary>
	sealed class EntityFSM : FiniteStateMachine<EntityStateID>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Supplies the properties of the entity whose state is managed by this FSM to the various states.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties
		{
			set
			{
				foreach(dynamic state in States)
				{
					state.EntityProperties = value;
				}
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity FSM directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the FSM.</param>
		public EntityFSM(IDictionary<string,dynamic> properties)
		:	base(properties)
		{
			Initialise();
		}

		/// <summary>
		/// Constructs an entity FSM from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the FSM's XML representation.</param>
		public EntityFSM(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the FSM using its properties.
		/// </summary>
		private void Initialise()
		{
			// Add the necessary states.
			AddState(EntityStateID.IN_CONSTRUCTION, new EntityInConstructionState(Properties["TimeElapsed"]));
			AddState(EntityStateID.OPERATING, new EntityOperatingState());

			// Add the necessary transitions.
			AddTransition(
				EntityStateID.IN_CONSTRUCTION,
				(EntityInConstructionState s) => s.PercentComplete >= 100 ? EntityStateID.OPERATING : EntityStateID.IN_CONSTRUCTION
			);

			// Set the starting state.
			EntityStateID currentStateID;
			if(Enum.TryParse(Properties["CurrentStateID"], out currentStateID))
			{
				CurrentStateID = currentStateID;
			}
			else
			{
				CurrentStateID = EntityStateID.OPERATING;
			}
		}

		#endregion
	}
}
