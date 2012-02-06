/***
 * game1666proto4: PlaceableEntityBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing a placeable entity.
	/// </summary>
	class PlaceableEntityBlueprint : Blueprint, ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The footprint for the type of entity to be built.
		/// </summary>
		public Footprint Footprint { get; private set; }

		/// <summary>
		/// The overall time required to construct the entity (in milliseconds).
		/// </summary>
		public int TimeToConstruct { get { return Properties["TimeToConstruct"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placeable entity blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public PlaceableEntityBlueprint(XElement blueprintElt)
		{
			Properties = EntityPersister.LoadProperties(blueprintElt);
			EntityPersister.LoadAndAddChildEntities(this, blueprintElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a footprint to the blueprint (note that a blueprint can only contain the one footprint).
		/// </summary>
		/// <param name="footprint">The footprint.</param>
		public void AddEntity(Footprint footprint)
		{
			Footprint = footprint;
		}

		#endregion
	}
}
