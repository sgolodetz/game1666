/***
 * game1666proto4: IOccupancyMap.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of a class implementing this interface provides occupancy information for a terrain.
	/// </summary>
	interface IOccupancyMap
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain for which occupancy information is being provided.
		/// </summary>
		Terrain Terrain { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not any of the specified grid squares are occupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to check.</param>
		/// <returns>true, if any of the grid squares are occupied, or false otherwise.</returns>
		bool AreOccupied(IEnumerable<Vector2i> gridSquares);

		/// <summary>
		/// Checks whether or not the specified grid square is occupied.
		/// </summary>
		/// <param name="gridSquare">The grid square to check.</param>
		/// <returns>true, if the grid square is occupied, or false otherwise.</returns>
		bool IsOccupied(Vector2i gridSquare);

		#endregion
	}
}
