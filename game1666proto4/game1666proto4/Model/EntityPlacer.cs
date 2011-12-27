/***
 * game1666proto4: EntityPlacer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
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
			IEnumerable<Vector2i> gridSquares = OverlaidGridSquares(footprint, hotspotPosition, terrain, true);
			if(gridSquares == null) return null;

			float fullSum = 0f;
			int count = 0;
			foreach(Vector2i s in gridSquares)
			{
				float sum = 0f;
				sum += terrain.Heightmap[s.Y, s.X];
				sum += terrain.Heightmap[s.Y, s.X + 1];
				sum += terrain.Heightmap[s.Y + 1, s.X];
				sum += terrain.Heightmap[s.Y + 1, s.X + 1];
				sum /= 4f;

				fullSum += sum;
				++count;
			}
			return count > 0 ? (float?)fullSum / count : null;
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
		public static IEnumerable<Vector2i> OverlaidGridSquares(Footprint footprint,
																Vector2i hotspotPosition,
																Terrain terrain,
																bool useFootprintOccupancy)
		{
			bool[,] occupancy = footprint.Occupancy;
			Vector2i offset = hotspotPosition - footprint.Hotspot;

			int footprintHeight = occupancy.GetLength(0);
			int footprintWidth = occupancy.GetLength(1);
			int gridHeight = terrain.Heightmap.GetLength(0) - 1;		// - 1 because grid height not heightmap height
			int gridWidth = terrain.Heightmap.GetLength(1) - 1;		// - 1 because grid width not heightmap width

			var gridSquares = new List<Vector2i>();
			for(int y = 0; y < footprintHeight; ++y)
			{
				int gridY = y + offset.Y;
				if(gridY < 0 || gridY >= gridHeight) return null;

				for(int x = 0; x < footprintWidth; ++x)
				{
					int gridX = x + offset.X;
					if(gridX < 0 || gridX >= gridWidth) return null;

					if(!useFootprintOccupancy || occupancy[y,x])
					{
						gridSquares.Add(new Vector2i(gridX, gridY));
					}
				}
			}
			return gridSquares;
		}

		#endregion
	}
}
