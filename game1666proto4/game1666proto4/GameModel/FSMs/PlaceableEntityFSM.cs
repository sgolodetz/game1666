/***
 * game1666proto4: PlaceableEntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;

namespace game1666proto4.GameModel.FSMs
{
	/// <summary>
	/// An instance of this class is used to manage the state of a placeable entity over time.
	/// </summary>
	sealed class PlaceableEntityFSM : FiniteStateMachine<PlaceableEntityStateID>, IPersistableEntity
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
		/// Constructs a placeable entity FSM directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the FSM.</param>
		public PlaceableEntityFSM(IDictionary<string,dynamic> properties)
		:	base(properties)
		{
			Initialise();
		}

		/// <summary>
		/// Constructs a placeable entity FSM from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the FSM's XML representation.</param>
		public PlaceableEntityFSM(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the FSM to XML.
		/// </summary>
		/// <returns>An XML representation of the FSM.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, Properties);
			return entityElt;
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
			AddState(PlaceableEntityStateID.IN_CONSTRUCTION, new PlaceableEntityInConstructionState(Properties));
			AddState(PlaceableEntityStateID.OPERATING, new PlaceableEntityOperatingState());
			AddState(PlaceableEntityStateID.IN_DESTRUCTION, new PlaceableEntityInDestructionState(Properties));

			// Add the necessary transitions.
			AddTransition
			(
				PlaceableEntityStateID.IN_CONSTRUCTION,
				(PlaceableEntityInConstructionState s) =>
					s.PercentComplete >= 100 ? PlaceableEntityStateID.OPERATING : PlaceableEntityStateID.IN_CONSTRUCTION
			);

			// Set the starting state.
			PlaceableEntityStateID currentStateID;
			if(Enum.TryParse(Properties["CurrentStateID"], out currentStateID))
			{
				CurrentStateID = currentStateID;
			}
			else
			{
				CurrentStateID = PlaceableEntityStateID.OPERATING;
			}
		}

		#endregion
	}
}
