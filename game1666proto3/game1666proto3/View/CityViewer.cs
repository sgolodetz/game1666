/***
 * game1666proto3: CityViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto3
{
	/// <summary>
	/// A viewer to draw a city on the city screen.
	/// </summary>
	sealed class CityViewer : IViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private City m_city;							/// the city being viewed
		private IndexBuffer m_terrainIndexBuffer;		/// the index buffer used when rendering the city terrain
		private VertexBuffer m_terrainVertexBuffer;		/// the vertex buffer used when rendering the city terrain
		private readonly Viewport m_viewport;			/// the viewport into which the city will be drawn

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public City City
		{
			get
			{
				return m_city;
			}

			set
			{
				m_city = value;

				if(m_terrainIndexBuffer != null)
				{
					m_terrainIndexBuffer.Dispose();
					m_terrainIndexBuffer = null;
				}

				if(m_terrainVertexBuffer != null)
				{
					m_terrainVertexBuffer.Dispose();
					m_terrainVertexBuffer = null;
				}
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a viewer that can be used to draw the specified city in the specified viewport.
		/// </summary>
		/// <param name="city">The city being viewed.</param>
		/// <param name="viewport">The viewport into which the city will be drawn.</param>
		public CityViewer(City city, Viewport viewport)
		{
			m_city = city;
			m_viewport = viewport;

			// Register input handlers.
			MouseEventManager.OnMouseMoved += OnMouseMoved;
			MouseEventManager.OnMousePressed += OnMousePressed;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the city.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="content">The content manager containing any textures to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect, ContentManager content)
		{
			// If we're not currently viewing a city, don't try and render anything.
			if(m_city == null) return;

			// Make sure that any vertex and index buffers needed to draw the city have been appropriately created.
			EnsureBuffersCreated(graphics);

			// Save the existing viewport and replace it with our one.
			Viewport savedViewport = graphics.Viewport;
			graphics.Viewport = m_viewport;

			// Actually draw the city.
			DrawTerrain(graphics, ref basicEffect, content);

			// Restore the original viewport.
			graphics.Viewport = savedViewport;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws the terrain on which the city is founded.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="content">The content manager containing any textures to use when drawing.</param>
		private void DrawTerrain(GraphicsDevice graphics, ref BasicEffect basicEffect, ContentManager content)
		{
			// Save the current state of the basic effect.
			BasicEffect savedBasicEffect = basicEffect.Clone() as BasicEffect;

			// Enable texturing.
			basicEffect.Texture = content.Load<Texture2D>("landscape");
			basicEffect.TextureEnabled = true;

			// Render the terrain as a triangle list.
			graphics.SetVertexBuffer(m_terrainVertexBuffer);
			graphics.Indices = m_terrainIndexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_terrainVertexBuffer.VertexCount, 0, m_terrainIndexBuffer.IndexCount / 3);
			}

			// Restore the basic effect to its saved state.
			basicEffect = savedBasicEffect;
		}

		/// <summary>
		/// Makes sure that any vertex and index buffers needed to draw the city have been appropriately created.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		private void EnsureBuffersCreated(GraphicsDevice graphics)
		{
			if(m_terrainVertexBuffer == null)
			{
				TerrainMesh mesh = m_city.TerrainMesh;

				// Construct the individual vertices for the terrain.
				int terrainHeight = mesh.Heightmap.Length;
				int terrainWidth = mesh.Heightmap[0].Length;
				int gridHeight = terrainHeight - 1;
				int gridWidth = terrainWidth - 1;

				var vertices = new VertexPositionTexture[terrainHeight * terrainWidth];

				int vertIndex = 0;
				for(int y = 0; y < terrainHeight; ++y)
				{
					for(int x = 0; x < terrainWidth; ++x)
					{
						var position = new Vector3(x * mesh.GridSquareWidth, y * mesh.GridSquareHeight, mesh.Heightmap[y][x]);
						var texCoords = new Vector2((float)x / gridWidth, (float)y / gridHeight);
						vertices[vertIndex] = new VertexPositionTexture(position, texCoords);
						++vertIndex;
					}
				}

				// Create the vertex buffer and fill it with the constructed vertices.
				m_terrainVertexBuffer = new VertexBuffer(graphics, typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
				m_terrainVertexBuffer.SetData(vertices);

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

				// Create the index buffer.
				m_terrainIndexBuffer = new IndexBuffer(graphics, typeof(int), indices.Length, BufferUsage.WriteOnly);
				m_terrainIndexBuffer.SetData(indices);
			}
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMouseMoved(MouseState state)
		{
			/*m_buildingToPlace = null;

			Viewport viewport = GraphicsDevice.Viewport;

			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), m_basicEffect.Projection, m_basicEffect.View, m_basicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Find the terrain triangle we're hovering over (if any).
			PickedTriangle? pickedTriangle = m_city.PickTerrainTriangle(ray);

			// If we're hovering over a terrain triangle, set up a temporary building there to show where we would build.
			if(pickedTriangle != null)
			{
				m_buildingToPlace = new Building(pickedTriangle.Value.Triangle);
			}*/
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			/*if(state.LeftButton == ButtonState.Pressed && m_buildingToPlace != null)
			{
				m_city.AddBuilding(m_buildingToPlace);
				m_buildingToPlace = null;
			}*/
		}

		#endregion
	}
}
