/***
 * game1666proto4: HouseBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	sealed class HouseBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The maximum number of people that can occupy this house.
		/// </summary>
		public int MaxOccupants
		{
			get
			{
				// TODO
				throw new System.NotImplementedException();
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a house blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintXml">The XML representation of the blueprint.</param>
		/// <returns>The house blueprint.</returns>
		public static HouseBlueprint LoadFromXml(string blueprintXml)
		{
			// TODO
			throw new System.NotImplementedException();
		}

		#endregion
	}
}
