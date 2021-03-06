﻿/***
 * game1666: Blueprint.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for an object.
	/// </summary>
	abstract class Blueprint
	{
		//#################### PROPERTIES ####################
		#region

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
		protected Blueprint(XElement blueprintElt)
		{
			Properties = PropertyPersister.LoadProperties(blueprintElt);
		}

		#endregion
	}
}
