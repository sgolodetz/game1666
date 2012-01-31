/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing an entity.
	/// </summary>
	abstract class Blueprint : ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the blueprint (not relevant).
		/// </summary>
		public IEnumerable<dynamic> Children { get { return new List<dynamic>(); } }

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
				return Type.GetType("game1666proto4.GameModel.Entities." + entityTypeName);
			}
		}

		/// <summary>
		/// The name of the 3D model for this blueprint.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The name of the blueprint.
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The properties of the blueprint.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public Blueprint(XElement blueprintElt)
		{
			Properties = EntityLoader.LoadProperties(blueprintElt);
			EntityLoader.LoadAndAddChildEntities(this, blueprintElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public abstract void AddDynamicEntity(dynamic entity);

		#endregion
	}
}
