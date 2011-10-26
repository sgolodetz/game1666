﻿/***
 * game1666proto2: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto2
{
	/// <summary>
	/// Represents a building in a city.
	/// </summary>
	sealed class Building
	{
		//#################### CONSTANTS ####################
		#region

		const int BUILDING_HEIGHT = 10;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		Triangle m_baseTriangle;
		IndexBuffer m_indexBuffer;
		VertexBuffer m_vertexBuffer;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building based on the specified triangle in the terrain mesh.
		/// </summary>
		/// <param name="baseTriangle">The triangle on which the building is based.</param>
		public Building(Triangle baseTriangle)
		{
			m_baseTriangle = baseTriangle;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the building.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect)
		{
			// Ensure that the vertex and index buffers for the building have been created.
			EnsureBuffersCreated(graphics);

			// Save the current state of the basic effect.
			BasicEffect savedBasicEffect = basicEffect.Clone() as BasicEffect;

			// Enable vertex colouring.
			basicEffect.VertexColorEnabled = true;

			// Render the building as a triangle list.
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

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Ensures that the vertex and index buffers for the building have been created.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		private void EnsureBuffersCreated(GraphicsDevice graphics)
		{
			if(m_vertexBuffer == null)
			{
				// Construct the individual vertices for the block representing the building.
				var vertices = new VertexPositionColor[]
				{
					new VertexPositionColor(m_baseTriangle.Vertices[0], Color.Red),
					new VertexPositionColor(m_baseTriangle.Vertices[1], Color.Green),
					new VertexPositionColor(m_baseTriangle.Vertices[2], Color.Blue),
					new VertexPositionColor(new Vector3(m_baseTriangle.Vertices[0].X, m_baseTriangle.Vertices[0].Y, BUILDING_HEIGHT), Color.Red),
					new VertexPositionColor(new Vector3(m_baseTriangle.Vertices[1].X, m_baseTriangle.Vertices[1].Y, BUILDING_HEIGHT), Color.Green),
					new VertexPositionColor(new Vector3(m_baseTriangle.Vertices[2].X, m_baseTriangle.Vertices[2].Y, BUILDING_HEIGHT), Color.Blue)
				};

				// Create the vertex buffer and fill it with the constructed vertices.
				m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
				m_vertexBuffer.SetData(vertices);

				// Construct the index array.
				var indices = new short[]
				{
					0, 1, 4
					// TODO
				};

				// Create the index buffer.
				m_indexBuffer = new IndexBuffer(graphics, typeof(short), indices.Length, BufferUsage.WriteOnly);
				m_indexBuffer.SetData(indices);
			}
		}

		#endregion
	}
}
