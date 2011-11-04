/***
 * game1666proto3: BuildingFootprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	/// <summary>
	/// Represents the grid-based 'footprint' of an entity on the terrain - i.e. the pattern
	/// of squares the entity will occupy on the grid.
	/// </summary>
	sealed class Footprint
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Tuple<int,int> m_hotspot;		/// the square in the pattern that will be under the user's mouse when placing the entity
		private readonly int[,] m_pattern;				/// the pattern of grid squares that the entity will occupy

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building footprint.
		/// </summary>
		/// <param name="pattern">The pattern of grid squares that the entity will occupy.</param>
		/// <param name="hotspot">The square in the pattern that will be under the user's mouse when placing the entity.</param>
		public Footprint(int[,] pattern, Tuple<int,int> hotspot)
		{
			m_pattern = pattern;
			m_hotspot = hotspot;
		}

		#endregion
	}
}
