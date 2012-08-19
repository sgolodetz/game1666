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
		/// Constructs a blank, named entity.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		public ModelEntity(string name)
		:	base(name)
		{}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		public ModelEntity(XElement entityElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(entityElt, fixedProperties)
		{}

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
