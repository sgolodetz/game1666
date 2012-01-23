/***
 * game1666proto4: Building.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : ICompositeEntity, IPlaceableEntity, IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the building.
		/// </summary>
		public float Altitude { get { return Properties["Altitude"]; } }

		/// <summary>
		/// The blueprint for the building.
		/// </summary>
		public Blueprint Blueprint { get; private set; }

		/// <summary>
		/// The sub-entities of the building (not relevant to the rest of the game).
		/// </summary>
		public IEnumerable<dynamic> Children { get { return new List<dynamic>(); } }

		/// <summary>
		/// Whether or not the building is destructible.
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
		/// The finite state machine for the building.
		/// </summary>
		public FiniteStateMachine<EntityStateID> FSM { get; private set; }

		/// <summary>
		/// The name of the building (must be unique within its playing area).
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The 2D axis-aligned orientation of the building.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The placement strategy for the building.
		/// </summary>
		public IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		/// <summary>
		/// The properties of the building.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a building directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the building.</param>
		/// <param name="initialStateID">The initial state of the building.</param>
		public Building(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		{
			Properties = properties;
			Initialise();

			// Construct and add the building's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["ConstructionDone"] = 0;	// this is a new building, so no construction has yet started
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			AddEntity(new EntityFSM(fsmProperties));
		}

		/// <summary>
		/// Constructs a building from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the building's XML representation.</param>
		public Building(XElement entityElt)
		{
			Properties = EntityLoader.LoadProperties(entityElt);
			Initialise();

			EntityLoader.LoadAndAddChildEntities(this, entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a finite state machine (FSM) to the building (note that there can only be one FSM).
		/// </summary>
		/// <param name="fsm">The FSM.</param>
		public void AddEntity(EntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = Properties;
		}

		/// <summary>
		/// Adds an entity to the building based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Makes a clone of this building that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public abstract IPlaceableEntity CloneNew();

		/// <summary>
		/// Updates the building based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the building from its properties.
		/// </summary>
		private void Initialise()
		{
			Properties["Self"] = this;
			Properties["Name"] = "building:" + Guid.NewGuid().ToString();
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion
	}
}
