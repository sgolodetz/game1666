/***
 * game1666proto4: Blueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a blueprint for building an entity.
	/// </summary>
	abstract class Blueprint : Entity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the 3D model for this blueprint.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

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
	}
}
