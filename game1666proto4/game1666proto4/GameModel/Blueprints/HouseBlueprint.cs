/***
 * game1666proto4: HouseBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for building a house.
	/// </summary>
	sealed class HouseBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The maximum number of people that can occupy a house constructed using this blueprint.
		/// </summary>
		public int MaxOccupants { get { return Properties["MaxOccupants"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a house blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public HouseBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
