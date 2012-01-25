/***
 * game1666proto4: PlaceableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
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
		public FiniteStateMachine<EntityStateID> FSM { get; protected set; }

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

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this entity that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public abstract IPlaceableEntity CloneNew();

		#endregion
	}
}
