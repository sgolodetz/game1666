/***
 * game1666proto: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto
{
	/// <summary>
	/// Represents a city.
	/// </summary>
	sealed class City
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly List<Building> m_buildings = new List<Building>();
        private VertexBuffer m_vertexBuffer;

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The new building to be added.</param>
		public void AddBuilding(Building building)
		{
			m_buildings.Add(building);
		}

		/// <summary>
		/// Draws the city.
		/// </summary>
        /// <param name="graphics">The graphics device to use for drawing.</param>
        /// <param name="basicEffect">The basic effect to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, BasicEffect basicEffect)
		{
			// If we have not yet set up the vertex buffer for the city plane, do so now.
            if(m_vertexBuffer == null)
            {
                // Construct the individual vertices at the corners of the (bounded) plane.
                var vertices = new VertexPositionColor[4];
                vertices[0] = new VertexPositionColor(new Vector3(-1, 1, 0), Color.Green);
                vertices[1] = new VertexPositionColor(new Vector3(-1, -1, 0), Color.Blue);
                vertices[2] = new VertexPositionColor(new Vector3(1, 1, 0), Color.Red);
                vertices[3] = new VertexPositionColor(new Vector3(1, -1, 0), Color.Cyan);

                // Create the vertex buffer and fill it with the constructed vertices.
                m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionColor), 4, BufferUsage.WriteOnly);
                m_vertexBuffer.SetData<VertexPositionColor>(vertices);
            }

            // Draw the city plane.
            graphics.SetVertexBuffer(m_vertexBuffer);
            foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }

			// Draw the buildings in the city.
			foreach(Building building in m_buildings)
			{
				building.Draw(graphics, basicEffect);
			}
		}

		#endregion
	}
}
