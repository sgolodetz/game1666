﻿/***
 * game1666proto4: PlaceableEntityBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing a placeable entity.
	/// </summary>
	abstract class PlaceableEntityBlueprint : Blueprint
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
		:	base(blueprintElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
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