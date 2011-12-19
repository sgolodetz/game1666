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
	abstract class CompositeEntity<T> : Entity where T : IEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		abstract protected IEnumerable<T> Children { get; }

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
		public CompositeEntity(IDictionary<string,string> properties)
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

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child entity to this entity based on its dynamic type.
		/// </summary>
		/// <param name="entity">The child entity.</param>
		abstract public void AddEntityDynamic(dynamic entity);

		#endregion
	}
}
