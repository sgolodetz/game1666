/***
 * game1666proto4: HouseBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	sealed class HouseBlueprint : EntityBlueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The maximum number of people that can occupy a house constructed using this blueprint.
		/// </summary>
		public int MaxOccupants
		{
			get
			{
				// TODO
				throw new System.NotImplementedException();
			}
		}

		/// <summary>
		/// The name of the house blueprint, e.g. "Dwelling".
		/// </summary>
		public string Name
		{
			get
			{
				// TODO
				throw new System.NotImplementedException();
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a house blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		/// <returns>The house blueprint.</returns>
		public HouseBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
