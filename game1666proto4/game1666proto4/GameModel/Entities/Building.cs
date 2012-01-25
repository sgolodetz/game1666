/***
 * game1666proto4: Building.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : PlaceableEntity, ICompositeEntity, IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities of the building (not relevant to the rest of the game).
		/// </summary>
		public IEnumerable<dynamic> Children { get { return new List<dynamic>(); } }

		/// <summary>
		/// The placement strategy for the building.
		/// </summary>
		public override IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

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
