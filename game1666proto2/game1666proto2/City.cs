/***
 * game1666proto2: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto2
{
	/// <summary>
	/// Represents a city.
	/// </summary>
	sealed class City
	{
		//#################### CONSTANTS ####################
		#region

		private const float GRID_SIZE = 5f;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		private IndexBuffer m_indexBuffer;
		private readonly int[][] m_terrainHeightmap;
		private Triangle[] m_terrainMesh;
		private VertexBuffer m_vertexBuffer;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city founded on the specified underlying terrain heightmap.
		/// </summary>
		/// <param name="terrainHeightmap">The underlying terrain heightmap.</param>
		public City(int[][] terrainHeightmap)
		{
			m_terrainHeightmap = terrainHeightmap;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the city.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">The landscape texture to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect, Texture2D landscapeTexture)
		{
			EnsureDataExists(graphics);
			DrawCityTerrain(graphics, ref basicEffect, landscapeTexture);
		}

		/// <summary>
		/// Determine the closest triangle in the terrain (if any) that is hit by the specified ray.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <returns>The picked triangle (if any).</returns>
		public PickedTriangle? PickTerrainTriangle(Ray ray)
		{
			// Make sure the terrain mesh has been created.
			if(m_terrainMesh == null) return null;

			// Find the closest triangle (if any) that is hit.
			float bestDistance = float.MaxValue;
			PickedTriangle? bestPickedTriangle = null;

			foreach(var triangle in m_terrainMesh)
			{
				float? distance = ray.Intersects(triangle);
				if(distance != null && distance < bestDistance)
				{
					bestPickedTriangle = new PickedTriangle(ray.Position + ray.Direction * distance.Value, triangle);
					bestDistance = distance.Value;
				}
			}

			return bestPickedTriangle;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws the city terrain.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">The landscape texture to use when drawing.</param>
		private void DrawCityTerrain(GraphicsDevice graphics, ref BasicEffect basicEffect, Texture2D landscapeTexture)
		{
			// Save the current state of the basic effect.
			BasicEffect savedBasicEffect = basicEffect.Clone() as BasicEffect;

			// Enable texturing.
			basicEffect.Texture = landscapeTexture;
			basicEffect.TextureEnabled = true;

			// Render the terrain as a triangle list.
			graphics.SetVertexBuffer(m_vertexBuffer);
			graphics.Indices = m_indexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_vertexBuffer.VertexCount, 0, m_indexBuffer.IndexCount / 3);
			}

			// Restore the basic effect to its saved state.
			basicEffect = savedBasicEffect;
		}

		/// <summary>
		/// Ensures that the vertex buffer, index buffer and triangle mesh for the city terrain have been created.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		private void EnsureDataExists(GraphicsDevice graphics)
		{
			if(m_vertexBuffer == null)
			{
				// Construct the individual vertices for the terrain.
				int terrainHeight = m_terrainHeightmap.Length;
				int terrainWidth = m_terrainHeightmap[0].Length;
				int gridHeight = terrainHeight - 1;
				int gridWidth = terrainWidth - 1;

				var vertices = new VertexPositionTexture[terrainHeight * terrainWidth];

				int vertIndex = 0;
				for(int y = 0; y < terrainHeight; ++y)
				{
					for(int x = 0; x < terrainWidth; ++x)
					{
						var position = new Vector3(x * GRID_SIZE, y * GRID_SIZE, m_terrainHeightmap[y][x]);
						var texCoords = new Vector2((float)x / gridWidth, (float)y / gridHeight);
						vertices[vertIndex] = new VertexPositionTexture(position, texCoords);
						++vertIndex;
					}
				}

				// Create the vertex buffer and fill it with the constructed vertices.
				m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
				m_vertexBuffer.SetData(vertices);

				// Construct the index array.
				var indices = new int[gridHeight * gridWidth * 6];	// 2 triangles per grid square x 3 vertices per triangle

				int indicesIndex = 0;
				for(int y = 0; y < gridHeight; ++y)
				{
					for(int x = 0; x < gridWidth; ++x)
					{
						int start = y * terrainWidth + x;
						indices[indicesIndex++] = start;
						indices[indicesIndex++] = start + 1;
						indices[indicesIndex++] = start + terrainWidth;
						indices[indicesIndex++] = start + 1;
						indices[indicesIndex++] = start + 1 + terrainWidth;
						indices[indicesIndex++] = start + terrainWidth;
					}
				}

				// Construct the index buffer.
				m_indexBuffer = new IndexBuffer(graphics, typeof(int), indices.Length, BufferUsage.WriteOnly);
				m_indexBuffer.SetData(indices);

				// Construct the triangle mesh.
				m_terrainMesh = new Triangle[gridHeight * gridWidth * 2];
				int meshIndex = 0;
				for(int i = 0; i < indices.Length; i += 3)
				{
					m_terrainMesh[meshIndex++] = new Triangle(
						vertices[indices[i]].Position,
						vertices[indices[i+1]].Position,
						vertices[indices[i+2]].Position
					);
				}
			}
		}

		#endregion
	}
}
