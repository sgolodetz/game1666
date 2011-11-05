/***
 * game1666proto3: PlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
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

		public EntityFootprint Footprint		{ get { return m_footprint; } }
		public IndexBuffer IndexBuffer			{ get; protected set; }
		public Vector2i Position				{ get { return m_position; } }
		protected TerrainMesh TerrainMesh		{ get { return m_terrainMesh; } }
		public VertexBuffer VertexBuffer		{ get; protected set; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the entity could be validly placed on the terrain mesh, given its footprint and position.
		/// </summary>
		/// <returns>true, if the entity could be validly placed, or false otherwise</returns>
		abstract public bool ValidateFootprint();

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		protected void ConstructBuffers(float entityHeight)
		{
			int gridHeight = m_terrainMesh.Heightmap.GetLength(0) - 1;
			int gridWidth = m_terrainMesh.Heightmap.GetLength(1) - 1;

			int [,] pattern = m_footprint.Pattern;
			int patternHeightPlusOne = pattern.GetLength(0) + 1;
			int patternWidthPlusOne = pattern.GetLength(1) + 1;

			Vector2i offset = m_position - m_footprint.Hotspot;

			// Construct the individual vertices for the building.
			var vertices = new VertexPositionColor[patternHeightPlusOne * patternWidthPlusOne * 2];
			int vertIndex = 0;
			for(int y = offset.Y; y < offset.Y + patternHeightPlusOne; ++y)
			{
				for(int x = offset.X; x < offset.X + patternWidthPlusOne; ++x)
				{
					float z = 0 <= x && x < gridWidth && 0 <= y && y < gridHeight ? z = m_terrainMesh.Heightmap[y,x] : 0f;
					vertices[vertIndex++] = new VertexPositionColor(new Vector3(x * m_terrainMesh.GridSquareWidth, y * m_terrainMesh.GridSquareHeight, z), Color.Red);
					vertices[vertIndex++] = new VertexPositionColor(new Vector3(x * m_terrainMesh.GridSquareWidth, y * m_terrainMesh.GridSquareHeight, z + entityHeight), Color.Red);
				}
			}

			// Create the vertex buffer and fill it with the constructed vertices.
			VertexBuffer = new VertexBuffer(RenderingDetails.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
			VertexBuffer.SetData(vertices);

			// TODO
		}

		#endregion
	}
}
