/***
 * game1666proto4: PlaceableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Matchmaking;
using game1666proto4.GameModel.PlacementStrategies;

namespace game1666proto4.GameModel.Core
{
	/// <summary>
	/// An instance of a class deriving from this one represents an entity that can be placed in a playing area.
	/// The major purpose of the class is to provide an implementation of the necessary properties in order to
	/// make it easier to add new types of placeable entity.
	/// </summary>
	abstract class PlaceableEntity : IPersistableEntity, IPlaceableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The footprint of the entity, suitably rotated to take account of its orientation.
		/// </summary>
		private readonly Footprint m_footprint;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		public float Altitude
		{
			get { return Properties["Altitude"]; }
			set { Properties["Altitude"] = value; }
		}

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		public dynamic Blueprint { get; protected set; }

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
		/// The entrances to the entity.
		/// </summary>
		public IEnumerable<Vector2i> Entrances
		{
			get
			{
				Vector2i offset = Position - m_footprint.Hotspot;
				return m_footprint.Entrances.Select(e => e + offset);
			}
		}

		/// <summary>
		/// The finite state machine for the entity.
		/// </summary>
		public PlaceableEntityFSM FSM { get; private set; }

		/// <summary>
		/// The resource matchmaker for the entity's playing area.
		/// </summary>
		public ResourceMatchmaker Matchmaker { protected get; set; }

		/// <summary>
		/// The name of the entity (must be unique within its playing area).
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The parent of the entity (if any) in its name tree (or null if this is the root of the tree).
		/// </summary>
		public INamedEntity Parent { get; set; }

		/// <summary>
		/// The persistable entities contained within the entity.
		/// </summary>
		public virtual IEnumerable<IPersistableEntity> Persistables
		{
			get
			{
				yield return FSM;
			}
		}

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
		protected PlaceableEntity(IDictionary<string,dynamic> properties, PlaceableEntityStateID initialStateID)
		{
			Properties = properties;
			Initialise();

			// Construct and add the entity's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["ConstructionDone"] = 0;	// this is a new entity, so no construction has yet started
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			AddEntity(new PlaceableEntityFSM(fsmProperties));

			// Determine the entity's (suitably rotated) footprint.
			m_footprint = Blueprint.Footprint.Rotated((int)Orientation);
		}

		/// <summary>
		/// Constructs a placeable entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		protected PlaceableEntity(XElement entityElt)
		{
			Properties = EntityPersister.LoadProperties(entityElt);
			Initialise();
			EntityPersister.LoadAndAddChildEntities(this, entityElt);

			// Determine the entity's (suitably rotated) footprint.
			m_footprint = Blueprint.Footprint.Rotated((int)Orientation);
		}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to this entity based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public abstract void AddDynamicEntity(dynamic entity);

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
		public void AddEntity(PlaceableEntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = Properties;
		}

		/// <summary>
		/// Gets a named entity directly contained within the current entity.
		/// </summary>
		/// <param name="name">The name of the entity to look up.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public virtual INamedEntity GetEntityByName(string name)
		{
			// Most placeable entities won't contain other entities; those that do can override this method.
			return null;
		}

		/// <summary>
		/// Saves the entity to XML.
		/// </summary>
		/// <returns>An XML representation of the entity.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, Properties);
			EntityPersister.SaveChildEntities(entityElt, Persistables);
			return entityElt;
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
