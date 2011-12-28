/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for building an entity.
	/// </summary>
	abstract class Blueprint : CompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The type of entity this blueprint can be used to build.
		/// </summary>
		public Type EntityType
		{
			get
			{
				Type blueprintType = this.GetType();
				string blueprintTypeName = blueprintType.Name;
				string entityTypeName = blueprintTypeName.Substring(0, blueprintTypeName.Length - "Blueprint".Length);
				return Type.GetType(blueprintType.Namespace + "." + entityTypeName);
			}
		}

		/// <summary>
		/// The footprint for the type of entity to be built.
		/// </summary>
		public Footprint Footprint { get; private set; }

		/// <summary>
		/// The name of the 3D model for this blueprint.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The overall time required to construct the entity (in milliseconds).
		/// </summary>
		public int TimeToConstruct { get { return Properties["TimeToConstruct"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public Blueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a footprint to the blueprint (note that a blueprint can only contain the one footprint).
		/// </summary>
		/// <param name="footprint">The footprint.</param>
		public void AddEntity(Footprint footprint)
		{
			Footprint = footprint;
		}

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
