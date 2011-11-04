/***
 * game1666proto3: TerrainMesh.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666proto3
{
	/// <summary>
	/// Represents a terrain mesh consisting of triangles in a regular grid pattern (i.e. two triangles per grid square).
	/// </summary>
	sealed class TerrainMesh : IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly float m_gridSquareHeight;	/// the height of a square in the terrain grid
		private readonly float m_gridSquareWidth;	/// the width of a square in the terrain grid
		private readonly float[,] m_heightmap;		/// a heightmap specifying the z heights of the corners of the grid squares
		private readonly Triangle[] m_triangles;	/// the triangles that make up the mesh

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public float GridSquareHeight	{ get { return m_gridSquareHeight; } }
		public float GridSquareWidth	{ get { return m_gridSquareWidth; } }
		public float[,] Heightmap		{ get { return m_heightmap; } }
		public Triangle[] Triangles		{ get { return m_triangles; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a terrain mesh from a heightmap.
		/// </summary>
		/// <param name="heightmap">A heightmap specifying the z heights of the corners of the grid squares.</param>
		/// <param name="gridSquareWidth">The width of each grid square.</param>
		/// <param name="gridSquareHeight">The height of each grid square.</param>
		public TerrainMesh(float[,] heightmap, float gridSquareWidth, float gridSquareHeight)
		{
			// Save the heightmap and the grid square dimensions - they'll be needed later to construct vertex and index buffers.
			m_heightmap = heightmap;
			m_gridSquareWidth = gridSquareWidth;
			m_gridSquareHeight = gridSquareHeight;

			// Determine the size of the grid and allocate a mesh triangle array of the appropriate size.
			int heightmapHeight = heightmap.GetLength(0);
			int heightmapWidth = heightmap.GetLength(1);
			int gridHeight = heightmapHeight - 1;
			int gridWidth = heightmapWidth - 1;

			m_triangles = new Triangle[gridHeight * gridWidth * 2];

			// Construct the individual mesh triangles - there will be two for each square in the grid.
			int triangleIndex = 0;
			for(int y=0; y<gridHeight; ++y)
			{
				for(int x=0; x<gridWidth; ++x)
				{
					float xOffset = x * gridSquareWidth, yOffset = y * gridSquareHeight;
					m_triangles[triangleIndex++] = new Triangle(
						new Vector3(xOffset, yOffset, heightmap[y,x]),
						new Vector3(xOffset + gridSquareWidth, yOffset, heightmap[y,x+1]),
						new Vector3(xOffset, yOffset + gridSquareHeight, heightmap[y+1,x])
					);
					m_triangles[triangleIndex++] = new Triangle(
						new Vector3(xOffset + gridSquareWidth, yOffset, heightmap[y,x+1]),
						new Vector3(xOffset + gridSquareWidth, yOffset + gridSquareHeight, heightmap[y+1,x+1]),
						new Vector3(xOffset, yOffset + gridSquareWidth, heightmap[y+1,x])
					);
				}
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Find the grid square containing the nearest mesh triangle (if any) hit by the specified ray.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <returns>The coordinates of the grid square (if any), or null otherwise.</returns>
		public Tuple<int,int> PickGridSquare(Ray ray)
		{
			// TODO
			return null;
		}

		// TODO
		public bool ValidateFootprint(Footprint footprint, Tuple<int,int> position, BuildingOrientation orientation)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
