/***
 * game1666proto3: PlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
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
		private readonly EntityOrientation m_orientation;	/// the orientation of the entity
		private readonly Tuple<int,int> m_position;			/// the position of the entity's hotspot
		private readonly TerrainMesh m_terrainMesh;			/// the terrain on which the entity will stand

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new placeable model entity with the specified footprint, position and orientation.
		/// </summary>
		/// <param name="footprint">The footprint of the entity.</param>
		/// <param name="position">The position of the entity's hotspot.</param>
		/// <param name="orientation">The orientation of the entity.</param>
		/// <param name="terrainMesh">The terrain on which the entity will stand.</param>
		protected PlaceableModelEntity(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation, TerrainMesh terrainMesh)
		{
			m_footprint = footprint;
			m_orientation = orientation;
			m_terrainMesh = terrainMesh;
			m_position = position;
		}

		#endregion

		//#################### PROPERTIES ####################
		#region

		public EntityFootprint Footprint		{ get { return m_footprint; } }
		public IndexBuffer IndexBuffer			{ get; protected set; }
		public EntityOrientation Orientation	{ get { return m_orientation; } }
		public Tuple<int,int> Position			{ get { return m_position; } }
		protected TerrainMesh TerrainMesh		{ get { return m_terrainMesh; } }
		public VertexBuffer VertexBuffer		{ get; protected set; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the entity could be validly placed on the terrain mesh, given its position and orientation.
		/// </summary>
		/// <returns>true, if the entity could be validly placed, or false otherwise</returns>
		abstract public bool ValidateFootprint();

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		protected void ConstructBuffers(float entityHeight)
		{
			int [,] pattern = m_footprint.Pattern;
			int height = pattern.GetLength(0) + 1;
			int width = pattern.GetLength(1) + 1;

			//Tuple<int,int> offset = m_position - m_footprint.Hotspot;

			// Construct the individual vertices for the building.
			var vertices = new VertexPositionColor[height * width * 2];
			int vertIndex = 0;
			for(int y=0; y<height; ++y)
			{
				for(int x=0; x<width; ++x)
				{
					//vertices[vertIndex++] = new VertexPositionColor(x * m_terrainMesh.GridSquareWidth, y * m_terrainMesh.GridSquareHeight, m_terrainMesh.Heightmap[
					// TODO
				}
			}

			// Create the vertex buffer and fill it with the constructed vertices.
			/*VertexBuffer = new VertexBuffer(RenderingDetails.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
			VertexBuffer.SetData(vertices);*/

			// TODO
		}

		#endregion
	}
}
