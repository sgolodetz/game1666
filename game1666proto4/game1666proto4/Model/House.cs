/***
 * game1666proto4: House.cs
 * Copyright 2011. All rights reserved.
 ***/

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
		/// Constructs a house at the specified position.
		/// </summary>
		/// <param name="position">The position (relative to the origin of the containing entity) of the house's hotspot.</param>
		public House(Vector2i position)
		:	base(position)
		{}

		#endregion
	}
}
