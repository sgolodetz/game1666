﻿/***
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
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">A landscape texture to use when drawing the city plane.</param>
		public void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect, Texture2D landscapeTexture)
		{
			EnsureVertexBufferCreated(graphics);
			DrawCityPlane(graphics, ref basicEffect, landscapeTexture);
			DrawBuildings(graphics, ref basicEffect);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws the buildings in the city.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		private void DrawBuildings(GraphicsDevice graphics, ref BasicEffect basicEffect)
		{
			foreach(Building building in m_buildings)
			{
				building.Draw(graphics, ref basicEffect);
			}
		}

		/// <summary>
		/// Draws the city plane.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">The landscape texture to use when drawing.</param>
		private void DrawCityPlane(GraphicsDevice graphics, ref BasicEffect basicEffect, Texture2D landscapeTexture)
		{
			// Save the current state of the basic effect.
			BasicEffect savedBasicEffect = basicEffect.Clone() as BasicEffect;

			// Enable texturing.
			basicEffect.Texture = landscapeTexture;
			basicEffect.TextureEnabled = true;

			// Render the plane as a triangle strip.
			graphics.SetVertexBuffer(m_vertexBuffer);
			foreach(EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphics.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
			}

			// Restore the basic effect to its saved state.
			basicEffect = savedBasicEffect;
		}

		/// <summary>
		/// Ensure that the vertex buffer for the city plane has been created.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		private void EnsureVertexBufferCreated(GraphicsDevice graphics)
		{
			if(m_vertexBuffer == null)
			{
				// Construct the individual vertices at the corners of the (bounded) plane.
				var vertices = new VertexPositionTexture[]
				{
					new VertexPositionTexture(new Vector3(-10, 10, 0), new Vector2(0, 0)),		// 0
					new VertexPositionTexture(new Vector3(-10, -10, 0), new Vector2(0, 1)),		// 1
					new VertexPositionTexture(new Vector3(10, 10, 0), new Vector2(1, 0)),		// 2
					new VertexPositionTexture(new Vector3(10, -10, 0), new Vector2(1, 1))		// 3
				};

				// Create the vertex buffer and fill it with the constructed vertices.
				m_vertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
				m_vertexBuffer.SetData(vertices);
			}
		}

		#endregion
	}
}
