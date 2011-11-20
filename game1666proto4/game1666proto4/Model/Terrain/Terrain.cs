/***
 * game1666proto4: Terrain.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	sealed class Terrain : ModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private float[,] m_heightmap;
		private bool[,] m_occupancy;
		private QuadtreeNode m_quadtreeRoot;

		#endregion

		//#################### PROPERTIES ####################
		#region

		public IndexBuffer IndexBuffer { get; private set; }
		public VertexBuffer VertexBuffer { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Terrain(float[,] heightmap)
		{
			Initialise(heightmap);
		}

		public Terrain(XElement entityElt)
		:	base(entityElt)
		{
			int gridWidth = Convert.ToInt32(Properties["Width"]);
			int gridHeight = Convert.ToInt32(Properties["Height"]);
			float[,] heightmap = new float[gridHeight,gridWidth];

			List<float> heightmapValues = Properties["Heightmap"].Split(new char[] { ',' }).Select(s => Convert.ToSingle(s.Trim())).ToList();

			int valueIndex = 0;
			for(int y = 0; y < gridHeight; ++y)
			{
				for(int x = 0; x < gridWidth; ++x)
				{
					heightmap[y,x] = heightmapValues[valueIndex++];
				}
			}

			Initialise(heightmap);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		private void ConstructBuffers()
		{
			// Construct the individual vertices for the terrain.
			int heightmapHeight = m_heightmap.GetLength(0);
			int heightmapWidth = m_heightmap.GetLength(1);
			int gridHeight = heightmapHeight - 1;
			int gridWidth = heightmapWidth - 1;

			var vertices = new VertexPositionTexture[heightmapHeight * heightmapWidth];

			int vertIndex = 0;
			for(int y = 0; y < heightmapHeight; ++y)
			{
				for(int x = 0; x < heightmapWidth; ++x)
				{
					var position = new Vector3(x * GameConfig.TERRAIN_SCALE_X, y * GameConfig.TERRAIN_SCALE_Y, m_heightmap[y,x] * GameConfig.TERRAIN_SCALE_Z);
					var texCoords = new Vector2((float)x / gridWidth, (float)y / gridHeight);
					vertices[vertIndex++] = new VertexPositionTexture(position, texCoords);
				}
			}

			// Create the vertex buffer and fill it with the constructed vertices.
			this.VertexBuffer = new VertexBuffer(Renderer.GraphicsDevice, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
			this.VertexBuffer.SetData(vertices);

			// Construct the index array.
			var indices = new short[gridHeight * gridWidth * 6];	// 2 triangles per grid square x 3 vertices per triangle

			int indicesIndex = 0;
			for(int y = 0; y < gridHeight; ++y)
			{
				for(int x = 0; x < gridWidth; ++x)
				{
					int start = y * heightmapWidth + x;
					indices[indicesIndex++] = (short)start;
					indices[indicesIndex++] = (short)(start + 1);
					indices[indicesIndex++] = (short)(start + heightmapWidth);
					indices[indicesIndex++] = (short)(start + 1);
					indices[indicesIndex++] = (short)(start + 1 + heightmapWidth);
					indices[indicesIndex++] = (short)(start + heightmapWidth);
				}
			}

			// Create the index buffer.
			this.IndexBuffer = new IndexBuffer(Renderer.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
			this.IndexBuffer.SetData(indices);
		}

		private void Initialise(float[,] heightmap)
		{
			m_heightmap = heightmap;
			m_occupancy = new bool[heightmap.GetLength(0) - 1, heightmap.GetLength(1) - 1];
			m_quadtreeRoot = QuadtreeCompiler.BuildQuadtree(heightmap);
			ConstructBuffers();
		}

		#endregion
	}
}
