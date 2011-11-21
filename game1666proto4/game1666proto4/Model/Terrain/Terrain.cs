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
	/// <summary>
	/// An instance of this class represents a heightmap-based terrain.
	/// </summary>
	sealed class Terrain : ModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private float[,] m_heightmap;			/// the heightmap for the terrain
		private bool[,] m_occupancy;			/// an occupancy grid indicating which grid squares are currently occupied, e.g. by buildings
		private QuadtreeNode m_quadtreeRoot;	/// the root node of the terrain quadtree (used for picking)

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain's index buffer (for use when rendering the terrain).
		/// </summary>
		public IndexBuffer IndexBuffer { get; private set; }

		/// <summary>
		/// The terrain's vertex buffer (for use when rendering the terrain).
		/// </summary>
		public VertexBuffer VertexBuffer { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a terrain from a heightmap.
		/// </summary>
		/// <param name="heightmap">The heightmap.</param>
		public Terrain(float[,] heightmap)
		{
			Initialise(heightmap);
		}

		/// <summary>
		/// Constructs a terrain from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the terrain's XML representation.</param>
		public Terrain(XElement entityElt)
		:	base(entityElt)
		{
			// Look up the width and height of the terrain grid in the properties loaded in from XML,
			// and construct an appropriately-sized heightmap.
			int heightmapWidth = Convert.ToInt32(Properties["Width"]);
			int heightmapHeight = Convert.ToInt32(Properties["Height"]);
			float[,] heightmap = new float[heightmapHeight,heightmapWidth];

			// Parse the heightmap values from the properties loaded in from XML.
			List<float> heightmapValues = Properties["Heightmap"].Split(new char[] { ',' }).Select(s => Convert.ToSingle(s.Trim())).ToList();

			// Fill in the heightmap with these values.
			int valueIndex = 0;
			for(int y = 0; y < heightmapHeight; ++y)
			{
				for(int x = 0; x < heightmapWidth; ++x)
				{
					heightmap[y,x] = heightmapValues[valueIndex++];
				}
			}

			// Use the heightmap to initialise the terrain.
			Initialise(heightmap);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Constructs the vertex and index buffers for the terrain (for use when rendering the terrain).
		/// </summary>
		private void ConstructBuffers()
		{
			int heightmapHeight = m_heightmap.GetLength(0);
			int heightmapWidth = m_heightmap.GetLength(1);
			int gridHeight = heightmapHeight - 1;
			int gridWidth = heightmapWidth - 1;

			// Construct the individual vertices for the terrain.
			var vertices = new VertexPositionTexture[heightmapHeight * heightmapWidth];

			int vertIndex = 0;
			for(int y = 0; y < heightmapHeight; ++y)
			{
				for(int x = 0; x < heightmapWidth; ++x)
				{
					var position = new Vector3(x, y, m_heightmap[y,x]) * GameConfig.TERRAIN_SCALE;
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

		/// <summary>
		/// Initialise the terrain from a heightmap.
		/// </summary>
		/// <param name="heightmap">The heightmap.</param>
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
