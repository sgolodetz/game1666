/***
 * game1666proto3: PlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// The base class for model entities that can be placed on the terrain.
	/// </summary>
	abstract class PlaceableModelEntity : IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly EntityFootprint m_footprint;		/// the footprint of the entity
		private readonly Vector2i m_position;				/// the position of the entity's hotspot
		private readonly TerrainMesh m_terrainMesh;			/// the terrain on which the entity will stand

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new placeable model entity with the specified footprint and position.
		/// </summary>
		/// <param name="footprint">The footprint of the entity.</param>
		/// <param name="position">The position of the entity's hotspot.</param>
		/// <param name="terrainMesh">The terrain on which the entity will stand.</param>
		protected PlaceableModelEntity(EntityFootprint footprint, Vector2i position, TerrainMesh terrainMesh)
		{
			m_footprint = footprint;
			m_terrainMesh = terrainMesh;
			m_position = position;
		}

		#endregion

		//#################### PROPERTIES ####################
		#region

		public bool CanPlace					{ get; private set; }
		public EntityFootprint Footprint		{ get { return m_footprint; } }
		public IndexBuffer IndexBuffer			{ get; protected set; }
		public Vector2i Position				{ get { return m_position; } }
		protected TerrainMesh TerrainMesh		{ get { return m_terrainMesh; } }
		public VertexBuffer VertexBuffer		{ get; protected set; }

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Constructs the vertex and index buffers for the entity.
		/// </summary>
		/// <param name="entityHeight">The height of the entity.</param>
		protected void ConstructBuffers(float entityHeight)
		{
			int heightmapHeight = m_terrainMesh.Heightmap.GetLength(0);
			int heightmapWidth = m_terrainMesh.Heightmap.GetLength(1);

			int [,] pattern = m_footprint.Pattern;
			int patternHeight = pattern.GetLength(0), patternWidth = pattern.GetLength(1);
			int patternHeightPlusOne = patternHeight + 1, patternWidthPlusOne = patternWidth + 1;

			// Construct the individual vertices for the entity.
			CanPlace = true;
			float minZ = float.MaxValue;
			float maxZ = float.MinValue;

			Vector2i offset = m_position - m_footprint.Hotspot;

			var vertices = new List<VertexPositionColor>();
			for(int y = offset.Y; y < offset.Y + patternHeightPlusOne; ++y)
			{
				for(int x = offset.X; x < offset.X + patternWidthPlusOne; ++x)
				{
					float z;
					if(0 <= x && x < heightmapWidth && 0 <= y && y < heightmapHeight)
					{
						z = m_terrainMesh.Heightmap[y,x];
					}
					else
					{
						throw new InvalidOperationException("The building would not stand fully on the terrain");
					}

					minZ = Math.Min(minZ, z);
					maxZ = Math.Max(maxZ, z);

					vertices.Add(new VertexPositionColor(new Vector3(x * m_terrainMesh.GridSquareWidth, y * m_terrainMesh.GridSquareHeight, z), Color.Blue));
					vertices.Add(new VertexPositionColor(new Vector3(x * m_terrainMesh.GridSquareWidth, y * m_terrainMesh.GridSquareHeight, z + entityHeight), Color.Green));
				}
			}

			// If the terrain isn't flat at the point at which we're trying to place the entity, prevent placement.
			if(Math.Abs(maxZ - minZ) > Constants.EPSILON)
			{
				CanPlace = false;

				// Change the colour of the building to red.
				vertices = vertices.Select(v => new VertexPositionColor(v.Position, Color.Red)).ToList();
			}

			// Create the vertex buffer and fill it with the constructed vertices.
			VertexBuffer = new VertexBuffer(RenderingDetails.GraphicsDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
			VertexBuffer.SetData(vertices.ToArray());

			// Construct the index array.
			var indices = new List<short>();
			for(int y = 0; y < patternHeight; ++y)
			{
				for(int x = 0; x < patternWidth; ++x)
				{
					// Only add triangles for enabled squares in the pattern.
					if(pattern[y,x] == 0) continue;

					// Front
					indices.Add(GetVertexIndex(x, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 1, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x+1, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 1, patternWidthPlusOne, patternHeightPlusOne));

					// Right
					indices.Add(GetVertexIndex(x+1, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 1, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x+1, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 1, patternWidthPlusOne, patternHeightPlusOne));

					// Back
					indices.Add(GetVertexIndex(x+1, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					// Left
					indices.Add(GetVertexIndex(x, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					// Top
					indices.Add(GetVertexIndex(x, y, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x+1, y, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y+1, 1, patternWidthPlusOne, patternHeightPlusOne));

					// Bottom
					indices.Add(GetVertexIndex(x, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 0, patternWidthPlusOne, patternHeightPlusOne));

					indices.Add(GetVertexIndex(x+1, y+1, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x+1, y, 0, patternWidthPlusOne, patternHeightPlusOne));
					indices.Add(GetVertexIndex(x, y, 0, patternWidthPlusOne, patternHeightPlusOne));
				}
			}

			// Create the index buffer.
			IndexBuffer = new IndexBuffer(RenderingDetails.GraphicsDevice, typeof(short), indices.Count, BufferUsage.WriteOnly);
			IndexBuffer.SetData(indices.ToArray());
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Gets the index (in the vertex buffer) of the relevant building vertex.
		/// </summary>
		/// <param name="x">The x coordinate of the vertex in the pattern.</param>
		/// <param name="y">The y coordinate of the vertex in the pattern.</param>
		/// <param name="z">The z coordinate of the vertex (0 indicates the one on the terrain, 1 indicates the one above it).</param>
		/// <param name="patternWidthPlusOne">The width of the pattern + 1.</param>
		/// <param name="patternHeightPlusOne">The height of the pattern + 1.</param>
		/// <returns>See above.</returns>
		private static short GetVertexIndex(int x, int y, int z, int patternWidthPlusOne, int patternHeightPlusOne)
		{
			return (short)((y * patternWidthPlusOne + x) * 2 + z);
		}

		#endregion
	}
}
