/***
 * game1666proto4: EntityPlacer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;

namespace game1666proto4
{
	/// <summary>
	/// This class is used to help place entities on a terrain.
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
			IEnumerable<Vector2i> gridSquares = footprint.OverlaidGridSquares(hotspotPosition, terrain, true);
			if(gridSquares != null && gridSquares.Any())
			{
				return terrain.DetermineAverageAltitude(gridSquares);
			}
			else
			{
				return null;
			}
		}

		#endregion
	}
}
