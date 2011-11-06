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
			CanPlace = true;

			int[,] pattern = footprint.Pattern;
			int heightmapHeight = terrainMesh.Heightmap.GetLength(0);
			int heightmapWidth = terrainMesh.Heightmap.GetLength(1);
			int patternHeight = pattern.GetLength(0), patternWidth = pattern.GetLength(1);
			int patternHeightPlusOne = patternHeight + 1, patternWidthPlusOne = patternWidth + 1;

			Vector2i offset = position - footprint.Hotspot;

			float minZ = float.MaxValue, maxZ = float.MinValue;

			for(int y = 0; y < patternHeight; ++y)
			{
				for(int x = 0; x < patternWidth; ++x)
				{
					// Only worry about squares that form part of the building footprint.
					if(pattern[y,x] == 0) continue;

					// Calculate the actual coordinates of the near corner of the square in the heightmap.
					int ax = x + offset.X, ay = y + offset.Y;

					if(0 <= ax && ax + 1 < heightmapWidth && 0 <= ay && ay + 1 < heightmapHeight)
					{
						minZ = Math.Min(minZ, terrainMesh.Heightmap[ay,ax]);
						minZ = Math.Min(minZ, terrainMesh.Heightmap[ay,ax+1]);
						minZ = Math.Min(minZ, terrainMesh.Heightmap[ay+1,ax]);
						minZ = Math.Min(minZ, terrainMesh.Heightmap[ay+1,ax+1]);

						maxZ = Math.Max(maxZ, terrainMesh.Heightmap[ay,ax]);
						maxZ = Math.Max(maxZ, terrainMesh.Heightmap[ay,ax+1]);
						maxZ = Math.Max(maxZ, terrainMesh.Heightmap[ay+1,ax]);
						maxZ = Math.Max(maxZ, terrainMesh.Heightmap[ay+1,ax+1]);
					}
					else
					{
						CanPlace = false;
						break;
					}
				}
			}

			if(Math.Abs(maxZ - minZ) > Constants.EPSILON)
			{
				CanPlace = false;
			}

			// Construct the vertex and index buffers for the building, colouring it depending
			// on whether or not it can be placed at this point.
			if(CanPlace)
			{
				ConstructBuffers(2f, Color.Green, Color.DarkGreen);
			}
			else
			{
				ConstructBuffers(2f, Color.Red, Color.DarkRed);
			}
		}

		#endregion
	}
}
