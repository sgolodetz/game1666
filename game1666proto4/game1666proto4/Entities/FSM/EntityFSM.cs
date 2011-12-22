/***
 * game1666proto4: EntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// The various possible states of an entity.
	/// </summary>
	enum EntityStateID
	{
		IN_CONSTRUCTION,
		OPERATING
	}

	/// <summary>
	/// An instance of this class is used to manage the state of an entity over time.
	/// </summary>
	sealed class EntityFSM : FiniteStateMachine<EntityStateID>
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity FSM from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the FSM's XML representation.</param>
		public EntityFSM(XElement entityElt)
		:	base(entityElt)
		{
			// Add the necessary states.
			AddState(EntityStateID.IN_CONSTRUCTION, new EntityInConstructionState(1000));
			AddState(EntityStateID.OPERATING, new EntityOperatingState());

			// Add the necessary transitions.
			AddTransition(EntityStateID.IN_CONSTRUCTION, (EntityInConstructionState s) =>
				s.PercentComplete >= 100 ? EntityStateID.OPERATING : EntityStateID.IN_CONSTRUCTION
			);

			// Set the starting state.
			EntityStateID currentStateID;
			CurrentStateID = Enum.TryParse(Properties["CurrentStateID"], out currentStateID) ? currentStateID : EntityStateID.OPERATING;
		}

		#endregion
	}
}
