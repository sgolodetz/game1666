/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// This class represents a blueprint for building an entity.
	/// </summary>
	abstract class Blueprint : ModelEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the blueprint, e.g. "Dwelling" for a house blueprint.
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the blueprint's XML representation.</param>
		public Blueprint(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
