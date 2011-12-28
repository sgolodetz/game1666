/***
 * game1666proto4: Terrain.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Graphics;
using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4.GameModel.Terrains
{
	/// <summary>
	/// An instance of this class represents a heightmap-based terrain.
	/// </summary>
	sealed class Terrain : Entity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The heightmap for the terrain.
		/// </summary>
		public float[,] Heightmap { get; private set; }

		/// <summary>
		/// The terrain's index buffer (for use when rendering the terrain).
		/// </summary>
		public IndexBuffer IndexBuffer { get; private set; }

		/// <summary>
		/// An occupancy grid indicating which grid squares are currently occupied, e.g. by buildings.
		/// </summary>
		public bool[,] Occupancy { get; private set; }

		/// <summary>
		/// The root node of the terrain quadtree (used for picking).
		/// </summary>
		public QuadtreeNode QuadtreeRoot { get; private set; }

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
			float[,] heightmap = null;
			if(Properties.ContainsKey("AssetHeightmap"))
			{
				// Load a heightmap from the specified XNA texture asset.
				heightmap = LoadHeightmapFromAsset(Properties["AssetHeightmap"]);
			}
			else
			{
				// Create a heightmap from the properties loaded in from XML.
				heightmap = CreateHeightmapFromProperties();
			}

			// Use the heightmap to initialise the terrain.
			Initialise(heightmap);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not any of the specified grid squares are occupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to check.</param>
		/// <returns>true, if any of the grid squares are occupied, or false otherwise</returns>
		public bool AreOccupied(IEnumerable<Vector2i> gridSquares)
		{
			return gridSquares.Any(s => Occupancy[s.Y, s.X]);
		}

		/// <summary>
		/// Calculates the height range over a non-empty set of grid squares.
		/// </summary>
		/// <param name="gridSquares">The grid squares.</param>
		/// <returns>The height range over the grid squares.</returns>
		public float CalculateHeightRange(IEnumerable<Vector2i> gridSquares)
		{
			float maxHeight = float.MinValue;
			float minHeight = float.MaxValue;

			foreach(Vector2i s in gridSquares)
			{
				maxHeight = Math.Max(Heightmap[s.Y, s.X], maxHeight);
				maxHeight = Math.Max(Heightmap[s.Y, s.X + 1], maxHeight);
				maxHeight = Math.Max(Heightmap[s.Y + 1, s.X], maxHeight);
				maxHeight = Math.Max(Heightmap[s.Y + 1, s.X + 1], maxHeight);

				minHeight = Math.Min(Heightmap[s.Y, s.X], minHeight);
				minHeight = Math.Min(Heightmap[s.Y, s.X + 1], minHeight);
				minHeight = Math.Min(Heightmap[s.Y + 1, s.X], minHeight);
				minHeight = Math.Min(Heightmap[s.Y + 1, s.X + 1], minHeight);
			}

			if(maxHeight >= minHeight) return maxHeight - minHeight;
			else throw new ArgumentException("Cannot determine the height range of an empty set of grid squares");
		}

		/// <summary>
		/// Determines the average altitude of a non-empty set of grid squares.
		/// </summary>
		/// <param name="gridSquares">The grid squares.</param>
		/// <returns>The average altitude of the grid squares.</returns>
		public float DetermineAverageAltitude(IEnumerable<Vector2i> gridSquares)
		{
			float fullSum = 0f;
			int count = 0;
			foreach(Vector2i s in gridSquares)
			{
				float sum = 0f;
				sum += Heightmap[s.Y, s.X];
				sum += Heightmap[s.Y, s.X + 1];
				sum += Heightmap[s.Y + 1, s.X];
				sum += Heightmap[s.Y + 1, s.X + 1];
				sum /= 4f;

				fullSum += sum;
				++count;
			}

			if(count > 0) return fullSum / count;
			else throw new ArgumentException("Cannot determine the average altitude of an empty set of grid squares");
		}

		/// <summary>
		/// Marks a set of grid squares as occupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to mark.</param>
		public void MarkOccupied(IEnumerable<Vector2i> gridSquares)
		{
			foreach(Vector2i s in gridSquares)
			{
				Occupancy[s.Y, s.X] = true;
			}
		}

		/// <summary>
		/// Finds the terrain grid square (if any) hit by the specified ray.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <returns>The nearest terrain grid square hit by the specified ray (if found), or null otherwise.</returns>
		public Vector2i? PickGridSquare(Ray ray)
		{
			if(ray.Intersects(QuadtreeRoot.Bounds) != null)
			{
				return QuadtreeRoot.PickGridSquare(ray);
			}
			else return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Create a heightmap from the properties loaded in from XML.
		/// </summary>
		/// <returns>The heightmap.</returns>
		private float[,] CreateHeightmapFromProperties()
		{
			// Look up the heightmap and required z-scaling in the properties loaded in from XML.
			float[,] heightmap = Properties["Heightmap"];
			float zScaling = Properties["ZScaling"];

			// Determine the dimensions of the heightmap.
			int height = heightmap.GetLength(0);
			int width = heightmap.GetLength(1);

			// Scale all of the elements of the heightmap by the z-scaling factor.
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					heightmap[y,x] *= zScaling;
				}
			}

			return heightmap;
		}

		/// <summary>
		/// Constructs the vertex and index buffers for the terrain (for use when rendering the terrain).
		/// </summary>
		private void ConstructBuffers()
		{
			int heightmapHeight = Heightmap.GetLength(0);
			int heightmapWidth = Heightmap.GetLength(1);
			int gridHeight = heightmapHeight - 1;
			int gridWidth = heightmapWidth - 1;

			// Construct the individual vertices for the terrain.
			var vertices = new VertexPositionTexture[heightmapHeight * heightmapWidth];

			int vertIndex = 0;
			for(int y = 0; y < heightmapHeight; ++y)
			{
				for(int x = 0; x < heightmapWidth; ++x)
				{
					var position = new Vector3(x, y, Heightmap[y,x]);
					var texCoords = new Vector2(x * 2f / heightmapWidth, y * 2f / heightmapHeight);
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
			Heightmap = heightmap;
			Occupancy = new bool[heightmap.GetLength(0) - 1, heightmap.GetLength(1) - 1];
			QuadtreeRoot = QuadtreeCompiler.BuildQuadtree(heightmap);
			ConstructBuffers();
		}

		/// <summary>
		/// Loads a heightmap from an XNA texture asset.
		/// </summary>
		/// <param name="assetName">The name of the asset.</param>
		/// <returns>The heightmap.</returns>
		private float[,] LoadHeightmapFromAsset(string assetName)
		{
			// Load the texture and get the heightmap data from it.
			Texture2D texture = Renderer.Content.Load<Texture2D>("Heightmaps/" + assetName);
			var heightmapValues = new Color[texture.Width * texture.Height];
			texture.GetData(heightmapValues);

			// Create the heightmap from the heightmap data.
			var heightmap = new float[texture.Height + 1, texture.Width + 1];
			float zScaling = Properties["ZScaling"];
			int valueIndex = 0;
			for(int y = 0; y < texture.Height; ++y)
			{
				for(int x = 0; x < texture.Width; ++x)
				{
					// Note: It's a greyscale image, so all the RGB components will be equal here.
					heightmap[y,x] = (float)heightmapValues[valueIndex++].R * zScaling;
				}
			}

			// Pad it to ensure that it's of size (2m+1) x (2n+1), for some m and n.
			for(int y = 0; y < texture.Height; ++y) heightmap[y,texture.Width] = heightmap[y,texture.Width-1];
			for(int x = 0; x < texture.Width; ++x) heightmap[texture.Height,x] = heightmap[texture.Height-1,x];

			return heightmap;
		}

		#endregion
	}
}
