/***
 * game1666proto4: House.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a house.
	/// </summary>
	sealed class House : Building
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a house directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the house.</param>
		/// <param name="initialStateID">The initial state of the house.</param>
		public House(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		:	base(properties, initialStateID)
		{}

		/// <summary>
		/// Constructs a house from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the house's XML representation.</param>
		public House(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this house that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new House(Properties, EntityStateID.IN_CONSTRUCTION);
		}

		#endregion
	}
}
