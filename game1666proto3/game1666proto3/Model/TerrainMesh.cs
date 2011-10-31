/***
 * game1666proto3: TerrainMesh.cs
 * Copyright 2011. All rights reserved.
 ***/

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

		private readonly Triangle[] m_triangles;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public Triangle[] Triangles
		{
			get
			{
				return m_triangles;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a terrain mesh from a heightmap.
		/// </summary>
		/// <param name="heightmap">A heightmap specifying the z heights of the corners of the grid squares.</param>
		/// <param name="gridSquareWidth">The width of each grid square.</param>
		/// <param name="gridSquareHeight">The height of each grid square.</param>
		public TerrainMesh(float[][] heightmap, float gridSquareWidth, float gridSquareHeight)
		{
			int heightmapHeight = heightmap.Length;
			int heightmapWidth = heightmap[0].Length;
			int gridHeight = heightmapHeight - 1;
			int gridWidth = heightmapWidth - 1;

			m_triangles = new Triangle[gridHeight * gridWidth * 2];

			int triangleIndex = 0;
			for(int y=0; y<heightmapHeight; ++y)
			{
				for(int x=0; x<heightmapWidth; ++x)
				{
					float xOffset = x * gridSquareWidth, yOffset = y * gridSquareHeight;
					m_triangles[triangleIndex++] = new Triangle(
						new Vector3(xOffset, yOffset, heightmap[y][x]),
						new Vector3(xOffset + gridSquareWidth, yOffset, heightmap[y][x+1]),
						new Vector3(xOffset, yOffset + gridSquareHeight, heightmap[y+1][x])
					);
					m_triangles[triangleIndex++] = new Triangle(
						new Vector3(xOffset + gridSquareWidth, yOffset, heightmap[y][x+1]),
						new Vector3(xOffset + gridSquareWidth, yOffset + gridSquareHeight, heightmap[y+1][x+1]),
						new Vector3(xOffset, yOffset + gridSquareWidth, heightmap[y+1][x])
					);
				}
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		// TODO: PickGridSquare

		#endregion
	}
}
