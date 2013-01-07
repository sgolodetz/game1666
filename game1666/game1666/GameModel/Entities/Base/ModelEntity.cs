/***
 * game1666: ModelEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;

namespace game1666.GameModel.Entities.Base
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game model.
	/// </summary>
	sealed class ModelEntity : Entity<ModelEntity>, IEquatable<ModelEntity>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override ModelEntity Self { get { return this; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blank, named model entity.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		public ModelEntity(string name)
		:	base(name)
		{}

		/// <summary>
		/// Constructs a model entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public ModelEntity(XElement entityElt)
		:	base(entityElt, null)
		{}

		/// <summary>
		/// Constructs a model entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public ModelEntity(XElement entityElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(entityElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Creates an entity based on the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype on which to base the entity.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		/// <returns>The entity, if the specified prototype exists, or null otherwise.</returns>
		public static ModelEntity CreateFromPrototype(string prototypeName, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		{
			XElement prototypeEntity = EntityPrototypeManager.GetPrototypeEntity(prototypeName);
			if(prototypeEntity == null) return null;

			var entity = new ModelEntity(prototypeEntity, fixedProperties);
			entity.Prototype = prototypeName;
			return entity;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not this entity is equal to another one.
		/// </summary>
		/// <param name="rhs">The other entity.</param>
		/// <returns>true, if the two entities are equal, or false otherwise.</returns>
		public bool Equals(ModelEntity rhs)
		{
			return object.ReferenceEquals(this, rhs);
		}

		#endregion
	}
}
