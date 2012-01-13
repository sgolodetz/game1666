/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Terrains;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a city.
	/// </summary>
	sealed class City : IPlaceableEntity, IPlayingArea, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		private readonly List<Building> m_buildings = new List<Building>();

		/// <summary>
		/// The properties of the city.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the city.
		/// </summary>
		public float Altitude { get { return m_properties["Altitude"]; } }

		/// <summary>
		/// The blueprint for the city.
		/// </summary>
		public Blueprint Blueprint { get; private set; }

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		public IEnumerable<Building> Buildings { get { return m_buildings; } }

		/// <summary>
		/// The sub-entities contained within the city.
		/// </summary>
		public IEnumerable<dynamic> Children { get { return m_buildings; } }

		/// <summary>
		/// The finite state machine for the city.
		/// </summary>
		public FiniteStateMachine<EntityStateID> FSM { get; private set; }

		/// <summary>
		/// The name of the city.
		/// </summary>
		public string Name
		{
			get
			{
				// FIXME: Cities need to be given names when they are created.
				dynamic name;
				return m_properties.TryGetValue("Name", out name) ? name : "";
			}
		}

		/// <summary>
		/// The 2D axis-aligned orientation of the city.
		/// </summary>
		public Orientation4 Orientation { get { return m_properties["Orientation"]; } }

		/// <summary>
		/// The placement strategy for the city.
		/// </summary>
		public IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the city's hotspot.
		/// </summary>
		public Vector2i Position { get { return m_properties["Position"]; } }

		/// <summary>
		/// The city's terrain.
		/// </summary>
		public Terrain Terrain	{ get; private set; }

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
			m_properties = properties;
			Initialise();

			// Construct and add the city's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			fsmProperties["TimeElapsed"] = 0;	// this is a new city, so no construction time has yet elapsed
			AddEntity(new EntityFSM(fsmProperties));
		}

		/// <summary>
		/// Constructs a city from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the city's XML representation.</param>
		public City(XElement entityElt)
		{
			m_properties = EntityLoader.LoadProperties(entityElt);
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
			m_buildings.Add(building);

			Terrain.MarkOccupied(
				building.PlacementStrategy.Place(
					Terrain,
					building.Blueprint.Footprint,
					building.Position,
					building.Orientation
				),
				true
			);
		}

		/// <summary>
		/// Adds a finite state machine (FSM) to the city (note that there can only be one FSM).
		/// </summary>
		/// <param name="fsm">The FSM.</param>
		public void AddEntity(EntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = m_properties;
		}

		/// <summary>
		/// Adds a terrain to the city (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			Terrain = terrain;
		}

		/// <summary>
		/// Adds a placeable entity to the city.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddPlaceableEntity(IPlaceableEntity entity)
		{
			AddDynamicEntity(entity);
		}

		/// <summary>
		/// Makes a clone of this city that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public IPlaceableEntity CloneNew()
		{
			return new City(m_properties, EntityStateID.IN_CONSTRUCTION);
		}

		/// <summary>
		/// Deletes an entity from the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeleteDynamicEntity(dynamic entity)
		{
			DeleteEntity(entity);
		}

		/// <summary>
		/// Deletes a building from the city.
		/// </summary>
		/// <param name="building">The building.</param>
		public void DeleteEntity(Building building)
		{
			m_buildings.Remove(building);

			Terrain.MarkOccupied(
				building.PlacementStrategy.Place(
					Terrain,
					building.Blueprint.Footprint,
					building.Position,
					building.Orientation
				),
				false
			);
		}

		/// <summary>
		/// Deletes a placeable entity from the city.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeletePlaceableEntity(IPlaceableEntity entity)
		{
			DeleteDynamicEntity(entity);
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
			Blueprint = BlueprintManager.GetBlueprint(m_properties["Blueprint"]);
		}

		#endregion
	}
}
