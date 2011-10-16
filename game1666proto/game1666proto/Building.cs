/***
 * game1666: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto
{
	/// <summary>
	/// Represents a building in a city. For the purposes of the prototype, each building
	/// will just be a block placed in a particular position on the city plane.
	/// </summary>
	class Building
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private Vector2 m_position;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified position on the city plane.
		/// </summary>
		/// <param name="position">The position of the building.</param>
		public Building(Vector2 position)
		{
			m_position = position;
		}

		#endregion
	}
}
