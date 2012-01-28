﻿/***
 * game1666proto4: PlaceableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class deriving from this one represents an entity that can be placed in a playing area.
	/// The major purpose of the class is to provide an implementation of the necessary properties in order to
	/// make it easier to add new types of placeable entity.
	/// </summary>
	abstract class PlaceableEntity : IPlaceableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		public float Altitude { get { return Properties["Altitude"]; } }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		public Blueprint Blueprint { get; protected set; }

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		public bool Destructible
		{
			get
			{
				dynamic destructible;
				return Properties.TryGetValue("Destructible", out destructible) ? destructible : true;
			}
		}

		/// <summary>
		/// The finite state machine for the entity.
		/// </summary>
		public FiniteStateMachine<EntityStateID> FSM { get; private set; }

		/// <summary>
		/// The name of the entity (must be unique within its playing area).
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The placement strategy for the entity.
		/// </summary>
		public abstract IPlacementStrategy PlacementStrategy { get; }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		/// <summary>
		/// The properties of the building.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placeable entity directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		/// <param name="initialStateID">The initial state of the entity.</param>
		public PlaceableEntity(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		{
			Properties = properties;
			Initialise();

			// Construct and add the entity's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["ConstructionDone"] = 0;	// this is a new entity, so no construction has yet started
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			AddEntity(new EntityFSM(fsmProperties));
		}

		/// <summary>
		/// Constructs a placeable entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public PlaceableEntity(XElement entityElt)
		{
			Properties = EntityLoader.LoadProperties(entityElt);
			Initialise();
		}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this entity that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public abstract IPlaceableEntity CloneNew();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a finite state machine (FSM) to the entity (note that there can only be one FSM).
		/// </summary>
		/// <param name="fsm">The FSM.</param>
		public void AddEntity(EntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = Properties;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the entity from its properties.
		/// </summary>
		private void Initialise()
		{
			Properties["Self"] = this;
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion
	}
}