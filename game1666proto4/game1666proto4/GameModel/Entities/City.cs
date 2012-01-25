/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;
using game1666proto4.GameModel.Terrains;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a city.
	/// </summary>
	sealed class City : PlaceableEntity, IPlayingArea, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The city's playing area.
		/// </summary>
		private readonly PlayingArea m_playingArea = new PlayingArea();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the city.
		/// </summary>
		public IEnumerable<dynamic> Children { get { return m_playingArea.Children; } }

		/// <summary>
		/// The placement strategy for the city.
		/// </summary>
		public override IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		/// <summary>
		/// The city's terrain.
		/// </summary>
		public Terrain Terrain	{ get { return m_playingArea.Terrain; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the city.</param>
		/// <param name="initialStateID">The initial state of the city.</param>
		public City(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		{
			Properties = properties;
			Initialise();

			// Construct and add the city's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["ConstructionDone"] = 0;	// this is a new city, so no construction has yet started
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			AddEntity(new EntityFSM(fsmProperties));
		}

		/// <summary>
		/// Constructs a city from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the city's XML representation.</param>
		public City(XElement entityElt)
		{
			Properties = EntityLoader.LoadProperties(entityElt);
			Initialise();

			EntityLoader.LoadAndAddChildEntities(this, entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The building.</param>
		public void AddEntity(Building building)
		{
			m_playingArea.AddEntity(building);
		}

		/// <summary>
		/// Adds a finite state machine (FSM) to the city (note that there can only be one FSM).
		/// </summary>
		/// <param name="fsm">The FSM.</param>
		public void AddEntity(EntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = Properties;
		}

		/// <summary>
		/// Adds a terrain to the city (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			m_playingArea.AddEntity(terrain);
		}

		/// <summary>
		/// Makes a clone of this city that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new City(Properties, EntityStateID.IN_CONSTRUCTION);
		}

		/// <summary>
		/// Deletes an entity from the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeleteDynamicEntity(dynamic entity)
		{
			m_playingArea.DeleteDynamicEntity(entity);
		}

		/// <summary>
		/// Gets an entity in the city by its (relative) path.
		/// </summary>
		/// <param name="path">The path to the entity.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count != 0) return null;
			else return this;
		}

		/// <summary>
		/// Updates the city based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);

			foreach(IUpdateableEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the city from its properties.
		/// </summary>
		private void Initialise()
		{
			Properties["Self"] = this;

			dynamic name;
			if(!Properties.TryGetValue("Name", out name))
			{
				Properties["Name"] = "city:" + Guid.NewGuid().ToString();
			}

			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion
	}
}
