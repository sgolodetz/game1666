/***
 * game1666: HomeBlueprint.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for the home component of an entity.
	/// </summary>
	sealed class HomeBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The maximum number of people that can occupy the home.
		/// </summary>
		public int MaxOccupants { get { return Properties["MaxOccupants"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a home blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public HomeBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
