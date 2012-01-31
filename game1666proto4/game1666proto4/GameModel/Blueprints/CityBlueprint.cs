/***
 * game1666proto4: CityBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for building a city.
	/// </summary>
	sealed class CityBlueprint : PlaceableEntityBlueprint
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public CityBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
