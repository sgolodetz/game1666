/***
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

		const int BUILDING_HEIGHT = 5;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Triangle m_baseTriangle;
		private IndexBuffer m_indexBuffer;
		private VertexBuffer m_vertexBuffer;

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

			// Switch the rasterizer state to wireframe with no culling.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;
			rasterizerState.FillMode = FillMode.WireFrame;
			graphics.RasterizerState = rasterizerState;

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

			// Switch the rasterizer state back to filled with clockwise culling.
			rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			rasterizerState.FillMode = FillMode.Solid;
			graphics.RasterizerState = rasterizerState;
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
					new VertexPositionColor(m_baseTriangle.Vertices[0] + new Vector3(0, 0, 0.1f), Color.Red),
					new VertexPositionColor(m_baseTriangle.Vertices[1] + new Vector3(0, 0, 0.1f), Color.Green),
					new VertexPositionColor(m_baseTriangle.Vertices[2] + new Vector3(0, 0, 0.1f), Color.Blue),
					new VertexPositionColor(m_baseTriangle.Vertices[0] + new Vector3(0, 0, BUILDING_HEIGHT), Color.White),
					new VertexPositionColor(m_baseTriangle.Vertices[1] + new Vector3(0, 0, BUILDING_HEIGHT), Color.White),
					new VertexPositionColor(m_baseTriangle.Vertices[2] + new Vector3(0, 0, BUILDING_HEIGHT), Color.White)
				};

				// Create the vertex buffer and fill it with the constructed vertices.
				m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
				m_vertexBuffer.SetData(vertices);

				// Construct the index array.
				var indices = new short[]
				{
					// Sides
					0, 1, 3,
					1, 4, 3,
					1, 2, 4,
					2, 5, 4,
					2, 0, 5,
					0, 3, 5,

					// Bottom
					0, 2, 1,

					// Top
					3, 4, 5
				};

				// Create the index buffer.
				m_indexBuffer = new IndexBuffer(graphics, typeof(short), indices.Length, BufferUsage.WriteOnly);
				m_indexBuffer.SetData(indices);
			}
		}

		#endregion
	}
}
