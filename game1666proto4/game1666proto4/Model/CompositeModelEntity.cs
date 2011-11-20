/***
 * game1666proto4: CompositeModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a composite entity in the game model, e.g. a city.
	/// </summary>
	abstract class CompositeModelEntity : ModelEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite entity without any properties.
		/// </summary>
		public CompositeModelEntity()
		{}

		/// <summary>
		/// Constructs a composite entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public CompositeModelEntity(IDictionary<string,string> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a composite entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeModelEntity(XElement entityElt)
		:	base(entityElt)
		{
			foreach(XElement childElt in entityElt.Elements("entity"))
			{
				// Look up the C# type of the child entity.
				string childTypename = "game1666proto4." + Convert.ToString(childElt.Attribute("type").Value);
				Type childType = Type.GetType(childTypename);

				if(childType != null)
				{
					// Construct the child entity and add it as a child of this one.
					AddEntityDynamic(Activator.CreateInstance(childType, childElt));
				}
				else
				{
					throw new InvalidOperationException("No such class: " + childTypename);
				}
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
