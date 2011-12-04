/***
 * game1666proto4: ViewEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a view entity in the game, e.g. a playing area viewer.
	/// </summary>
	abstract class ViewEntity : Entity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity without any properties.
		/// </summary>
		public ViewEntity()
		:	base()
		{}

		/// <summary>
		/// Constructs an entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public ViewEntity(IDictionary<string,string> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public ViewEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity.
		/// </summary>
		public abstract void Draw();

		#endregion
	}
}
