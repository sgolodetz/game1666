/***
 * game1666proto4: EntityPlacer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4
{
	/// <summary>
	/// This class is used to place entities on a terrain.
	/// </summary>
	static class EntityPlacer
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines the average altitude of the grid squares overload by the specified footprint
		/// when placed at the specified position on a terrain.
		/// </summary>
		/// <param name="footprint">The footprint.</param>
		/// <param name="hotspotPosition">The position (on the terrain) of the footprint's hotspot.</param>
		/// <param name="terrain">The terrain.</param>
		/// <returns>The average altitude of the grid squares, if the footprint is validly placed, or null otherwise</returns>
		public static float? DetermineAverageAltitude(Footprint footprint, Vector2i hotspotPosition, Terrain terrain)
		{
			// TODO
			return 0f;
		}

		/// <summary>
		/// Attempts to place a building on a terrain at the specified position.
		/// </summary>
		/// <param name="building">The building.</param>
		/// <param name="hotspotPosition">The position (on the terrain) of the building footprint's hotspot.</param>
		/// <param name="terrain">The terrain.</param>
		/// <returns>A set of overlaid grid squares, if the building is validly placed, or null otherwise</returns>
		public static IEnumerable<Vector2i> Place(Building building, Vector2i hotspotPosition, Terrain terrain)
		{
			Footprint footprint = building.Blueprint.Footprint;
			if(CheckFlatness(OverlaidGridSquares(footprint, hotspotPosition, terrain, false)))
			{
				return OverlaidGridSquares(footprint, hotspotPosition, terrain, true);
			}
			else return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the specified grid squares are all flat and at the same altitude.
		/// </summary>
		/// <param name="gridSquares">The grid squares.</param>
		/// <returns>true, if they are all flat and at the same altitude, or false otherwise</returns>
		private static bool CheckFlatness(IEnumerable<Vector2i> gridSquares)
		{
			// TODO
			return true;
		}

		/// <summary>
		/// Determines which grid squares in a terrain are overlaid by the specified entity footprint
		/// placed at the specified position.
		/// </summary>
		/// <param name="footprint">The footprint.</param>
		/// <param name="hotspotPosition">The position (on the terrain) of the footprint's hotspot.</param>
		/// <param name="terrain">The terrain.</param>
		/// <param name="useFootprintOccupancy">Whether or not to take footprint occupancy into account.</param>
		/// <returns>A set of overlaid grid squares, if the footprint is validly placed, or null otherwise</returns>
		private static IEnumerable<Vector2i> OverlaidGridSquares(Footprint footprint,
																 Vector2i hotspotPosition,
																 Terrain terrain,
																 bool useFootprintOccupancy)
		{
			// TODO
			return null;
		}

		#endregion
	}
}
