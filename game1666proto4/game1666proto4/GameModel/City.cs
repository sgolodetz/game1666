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

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a city.
	/// </summary>
	sealed class City : PlayingArea, IPlaceableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		private readonly List<Building> m_buildings = new List<Building>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the city.
		/// </summary>
		public float Altitude { get { return Properties["Altitude"]; } }

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
		protected override IEnumerable<IUpdateableEntity> Children { get { return m_buildings; } }

		/// <summary>
		/// The finite state machine for the city.
		/// </summary>
		public FiniteStateMachine<EntityStateID> FSM { get; private set; }

		/// <summary>
		/// The 2D axis-aligned orientation of the city.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the city's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the city's XML representation.</param>
		public City(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The building.</param>
		public void AddEntity(Building building)
		{
			m_buildings.Add(building);
			Terrain.MarkOccupied(building.Place(Terrain));
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
		/// Adds an entity to the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Makes a clone of this city that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public IPlaceableEntity CloneNew()
		{
			// TODO
			return null;
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
		/// Checks whether or not the city can be validly placed on the specified terrain,
		/// bearing in mind its position and orientation.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>true, if it can be validly placed, or false otherwise</returns>
		public bool IsValidlyPlaced(Terrain terrain)
		{
			// TODO
			return false;
		}

		/// <summary>
		/// Attempts to place the city on the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>A set of grid squares that the city overlays, if it can be validly placed, or null otherwise</returns>
		public IEnumerable<Vector2i> Place(Terrain terrain)
		{
			// TODO
			return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the city from its properties.
		/// </summary>
		private void Initialise()
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion
	}
}
