/***
 * game1666proto3: TerrainMesh.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// Represents a terrain mesh consisting of triangles in a regular grid pattern (i.e. two triangles per grid square).
	/// </summary>
	sealed class TerrainMesh : IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly int m_gridHeight;				/// the height of the grid (in squares)
		private readonly int m_gridWidth;				/// the width of the grid (in squares)

		private readonly float m_gridSquareHeight;		/// the height of a square in the terrain grid
		private readonly float m_gridSquareWidth;		/// the width of a square in the terrain grid

		private readonly float[,] m_heightmap;			/// a heightmap specifying the z heights of the corners of the grid squares

		private readonly IndexBuffer m_indexBuffer;		/// the index buffer to use when rendering the terrain mesh
		private readonly VertexBuffer m_vertexBuffer;	/// the vertex buffer to use when rendering the terrain mesh

		private readonly Triangle[] m_triangles;		/// the triangles that make up the mesh

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public float GridSquareHeight		{ get { return m_gridSquareHeight; } }
		public float GridSquareWidth		{ get { return m_gridSquareWidth; } }
		public float[,] Heightmap			{ get { return m_heightmap; } }
		public IndexBuffer IndexBuffer		{ get { return m_indexBuffer; } }
		public Triangle[] Triangles			{ get { return m_triangles; } }
		public VertexBuffer VertexBuffer	{ get { return m_vertexBuffer; } }

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
			// Save the heightmap and the grid square dimensions for later use.
			m_heightmap = heightmap;
			m_gridSquareWidth = gridSquareWidth;
			m_gridSquareHeight = gridSquareHeight;

			// Construct the individual vertices for the terrain.
			int heightmapHeight = heightmap.GetLength(0);
			int heightmapWidth = heightmap.GetLength(1);
			m_gridHeight = heightmapHeight - 1;
			m_gridWidth = heightmapWidth - 1;

			var vertices = new VertexPositionTexture[heightmapHeight * heightmapWidth];

			int vertIndex = 0;
			for(int y = 0; y < heightmapHeight; ++y)
			{
				for(int x = 0; x < heightmapWidth; ++x)
				{
					var position = new Vector3(x * gridSquareWidth, y * gridSquareHeight, heightmap[y,x]);
					var texCoords = new Vector2((float)x / m_gridWidth, (float)y / m_gridHeight);
					vertices[vertIndex] = new VertexPositionTexture(position, texCoords);
					++vertIndex;
				}
			}

			// Create the vertex buffer and fill it with the constructed vertices.
			m_vertexBuffer = new VertexBuffer(RenderingDetails.GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
			m_vertexBuffer.SetData(vertices);

			// Construct the index array.
			var indices = new int[m_gridHeight * m_gridWidth * 6];	// 2 triangles per grid square x 3 vertices per triangle

			int indicesIndex = 0;
			for(int y = 0; y < m_gridHeight; ++y)
			{
				for(int x = 0; x < m_gridWidth; ++x)
				{
					int start = y * heightmapWidth + x;
					indices[indicesIndex++] = start;
					indices[indicesIndex++] = start + 1;
					indices[indicesIndex++] = start + heightmapWidth;
					indices[indicesIndex++] = start + 1;
					indices[indicesIndex++] = start + 1 + heightmapWidth;
					indices[indicesIndex++] = start + heightmapWidth;
				}
			}

			// Create the index buffer.
			m_indexBuffer = new IndexBuffer(RenderingDetails.GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
			m_indexBuffer.SetData(indices);

			// Construct the individual mesh triangles - there will be two for each square in the grid.
			m_triangles = new Triangle[m_gridHeight * m_gridWidth * 2];
			int meshIndex = 0;
			for(int i = 0; i < indices.Length; i += 3)
			{
				m_triangles[meshIndex++] = new Triangle(
					vertices[indices[i]].Position,
					vertices[indices[i+1]].Position,
					vertices[indices[i+2]].Position
				);
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
			float bestDistance = float.MaxValue;
			int? bestPickedTriangle = null;

			for(int i = 0; i < m_triangles.Length; ++i)
			{
				float? distance = ray.Intersects(m_triangles[i]);
				if(distance != null && distance < bestDistance)
				{
					bestPickedTriangle = i;
					bestDistance = distance.Value;
				}
			}

			if(bestPickedTriangle != null)
			{
				int rowTriangleCount = m_gridWidth * 2;
				int x = (bestPickedTriangle.Value % rowTriangleCount) / 2;
				int y = bestPickedTriangle.Value / rowTriangleCount;
				return Tuple.Create(x, y);
			}
			else return null;
		}

		#endregion
	}
}
