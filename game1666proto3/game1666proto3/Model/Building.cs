/***
 * game1666proto3: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// Represents a building in the game model.
	/// </summary>
	sealed class Building : PlaceableModelEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified footprint and position on the terrain.
		/// </summary>
		/// <param name="footprint">The footprint of the building.</param>
		/// <param name="position">The position of the building's hotspot.</param>
		/// <param name="terrainMesh">The terrain on which the building will stand.</param>
		public Building(EntityFootprint footprint, Vector2i position, TerrainMesh terrainMesh)
		:	base(footprint, position, terrainMesh)
		{
			// Check whether the building can be placed on the terrain at this point.
			this.CanPlace = IsPlaceable(footprint, position, terrainMesh);

			// Construct the vertex and index buffers for the building, colouring it depending
			// on whether or not it can be placed at this point.
			if(this.CanPlace)	ConstructBuffers(2f, Color.Green, Color.DarkGreen);
			else				ConstructBuffers(2f, Color.Red, Color.DarkRed);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Checks whether a building footprint is placeable at the specified position on a terrain mesh.
		/// </summary>
		/// <param name="footprint">The building footprint.</param>
		/// <param name="position">The position.</param>
		/// <param name="terrainMesh">The terrain mesh.</param>
		/// <returns>true, if the building footprint can be placed, or false otherwise</returns>
		private static bool IsPlaceable(EntityFootprint footprint, Vector2i position, TerrainMesh terrainMesh)
		{
			// Get the terrain heightmap and its dimensions.
			float[,] heightmap = terrainMesh.Heightmap;
			int heightmapHeight = heightmap.GetLength(0);
			int heightmapWidth = heightmap.GetLength(1);

			// Get the footprint pattern and its dimensions.
			int[,] pattern = footprint.Pattern;
			int patternHeight = pattern.GetLength(0), patternWidth = pattern.GetLength(1);
			int patternHeightPlusOne = patternHeight + 1, patternWidthPlusOne = patternWidth + 1;

			// Determine the offset needed to translate pattern coordinates into heightmap coordinates.
			Vector2i offset = position - footprint.Hotspot;

			// Determine the minimum and maximum heights of all the corners of the grid squares on which
			// the building would be placed.
			float minZ = float.MaxValue, maxZ = float.MinValue;
			for(int y = 0; y < patternHeight; ++y)
			{
				for(int x = 0; x < patternWidth; ++x)
				{
					// Only worry about grid squares that form part of the building footprint.
					if(pattern[y,x] == 0) continue;

					// Calculate the actual coordinates of the near corner of the grid square in the heightmap.
					int ax = x + offset.X, ay = y + offset.Y;

					// Check that the *entire* grid square is within the heightmap.
					if(0 <= ax && ax + 1 < heightmapWidth && 0 <= ay && ay + 1 < heightmapHeight)
					{
						// Update the minimum and maximum heights using all four corners of the grid square.
						minZ = Math.Min(minZ, heightmap[ay,ax]);		maxZ = Math.Max(maxZ, heightmap[ay,ax]);
						minZ = Math.Min(minZ, heightmap[ay,ax+1]);		maxZ = Math.Max(maxZ, heightmap[ay,ax+1]);
						minZ = Math.Min(minZ, heightmap[ay+1,ax]);		maxZ = Math.Max(maxZ, heightmap[ay+1,ax]);
						minZ = Math.Min(minZ, heightmap[ay+1,ax+1]);	maxZ = Math.Max(maxZ, heightmap[ay+1,ax+1]);
					}
					else
					{
						// If the grid square is outside the heightmap, the building would not
						// lie fully within the terrain and so cannot be placed.
						return false;
					}
				}
			}

			// Only allow building placement if the minimum and maximum heights are essentially
			// the same, i.e. the terrain is flat.
			return Math.Abs(maxZ - minZ) <= Constants.EPSILON;
		}

		#endregion
	}
}
