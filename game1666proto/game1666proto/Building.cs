/***
 * game1666proto: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto
{
	/// <summary>
	/// Represents a building in a city. For the purposes of the prototype, each building
	/// will just be a block placed in a particular position on the city plane.
	/// </summary>
	sealed class Building
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private IndexBuffer m_indexBuffer;
		private readonly Vector2 m_position;
		private VertexBuffer m_vertexBuffer;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified position on the city plane.
		/// </summary>
		/// <param name="position">The position of the building.</param>
		public Building(Vector2 position)
		{
			m_position = position;
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
			EnsureBuffersCreated(graphics);

			BasicEffect savedBasicEffect = basicEffect.Clone() as BasicEffect;

			// Enable vertex colouring.
			basicEffect.VertexColorEnabled = true;

			// Translate to the right place on the city plane.
			basicEffect.World = Matrix.CreateTranslation(new Vector3(m_position, 0));

			// Render the building as a triangle list.
			graphics.SetVertexBuffer(m_vertexBuffer);
			graphics.Indices = m_indexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_vertexBuffer.VertexCount, 0, m_indexBuffer.IndexCount / 3);
			}

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
			if (m_vertexBuffer == null)
			{
				// Construct the individual vertices for the block representing the building.
				var vertices = new VertexPositionColor[]
				{
					new VertexPositionColor(new Vector3(1, 1, 0), Color.Red),       // 0
					new VertexPositionColor(new Vector3(-1, 1, 0), Color.Green),    // 1
					new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue),    // 2
					new VertexPositionColor(new Vector3(1, -1, 0), Color.Cyan),     // 3
					new VertexPositionColor(new Vector3(1, 1, 5), Color.Magenta),   // 4
					new VertexPositionColor(new Vector3(-1, 1, 5), Color.Yellow),   // 5
					new VertexPositionColor(new Vector3(-1, -1, 5), Color.Orange),  // 6
					new VertexPositionColor(new Vector3(1, -1, 5), Color.Purple)    // 7
				};

				// Create the vertex buffer and fill it with the constructed vertices.
				m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
				m_vertexBuffer.SetData(vertices);

				// Construct the index array.
				var indices = new short[]
				{
					// Bottom
					3, 2, 1,
					0, 3, 1,

					// Right
					4, 7, 3,
					0, 4, 3,

					// Back
					5, 4, 0,
					1, 5, 0,

					// Left
					6, 5, 1,
					2, 6, 1,

					// Front
					7, 6, 2,
					3, 7, 2,

					// Top
					4, 5, 6,
					7, 4, 6
				};

				// Construct the index buffer.
				m_indexBuffer = new IndexBuffer(graphics, typeof(short), indices.Length, BufferUsage.WriteOnly);
				m_indexBuffer.SetData(indices);
			}
		}

		#endregion
	}
}
