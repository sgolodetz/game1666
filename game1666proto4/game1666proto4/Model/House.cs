/***
 * game1666proto4: House.cs
 * Copyright 2011. All rights reserved.
 ***/

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
		/// Constructs a house from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the house's XML representation.</param>
		public House(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
