/***
 * game1666proto4: CompositeEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a composite entity in the game, e.g. a city.
	/// </summary>
	abstract class CompositeEntity : Entity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite entity without any properties.
		/// </summary>
		public CompositeEntity()
		{}

		/// <summary>
		/// Constructs a composite entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public CompositeEntity(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a composite entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeEntity(XElement entityElt)
		:	base(entityElt)
		{
			foreach(dynamic child in EntityUtil.LoadChildEntities(entityElt))
			{
				AddEntityDynamic(child);
			}
		}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Adds a child entity to this entity based on its dynamic type.
		/// </summary>
		/// <param name="entity">The child entity.</param>
		public abstract void AddEntityDynamic(dynamic entity);

		#endregion
	}

	/// <summary>
	/// An instance of this class represents a composite entity in the game whose children can be enumerated.
	/// </summary>
	abstract class CompositeEntity<Child> : CompositeEntity where Child : IEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		protected abstract IEnumerable<Child> Children { get; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite entity without any properties.
		/// </summary>
		public CompositeEntity()
		{}

		/// <summary>
		/// Constructs a composite entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public CompositeEntity(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a composite entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
