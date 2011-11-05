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

		private readonly City m_city;					/// the city being viewed
		private PlaceableModelEntity m_entityToPlace;	/// the entity currently being placed by the user (if any)
		private readonly Viewport m_viewport;			/// the viewport into which the city will be drawn

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
		public void Draw()
		{
			// If we're not currently viewing a city, don't try and render anything.
			if(m_city == null) return;

			// Save the existing viewport and replace it with our one.
			Viewport savedViewport = RenderingDetails.GraphicsDevice.Viewport;
			RenderingDetails.GraphicsDevice.Viewport = m_viewport;

			// Actually draw the city.
			DrawTerrain();

			foreach(IModelEntity entity in m_city.GetEntities())
			{
				DrawEntity((dynamic)entity);
			}

			if(m_entityToPlace != null && m_entityToPlace.ValidateFootprint())
			{
				DrawEntity((dynamic)m_entityToPlace);
			}

			// Restore the original viewport.
			RenderingDetails.GraphicsDevice.Viewport = savedViewport;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws a building in the city.
		/// </summary>
		/// <param name="building">The building.</param>
		private void DrawEntity(Building building)
		{
			// TODO
		}

		/// <summary>
		/// Draws the terrain on which the city is founded.
		/// </summary>
		private void DrawTerrain()
		{
			BasicEffect basicEffect = RenderingDetails.BasicEffect.Clone() as BasicEffect;

			// Enable texturing.
			basicEffect.Texture = RenderingDetails.Content.Load<Texture2D>("landscape");
			basicEffect.TextureEnabled = true;

			// Render the terrain as a triangle list.
			RenderingDetails.GraphicsDevice.SetVertexBuffer(m_city.TerrainMesh.VertexBuffer);
			RenderingDetails.GraphicsDevice.Indices = m_city.TerrainMesh.IndexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				RenderingDetails.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, m_city.TerrainMesh.VertexBuffer.VertexCount, 0, m_city.TerrainMesh.IndexBuffer.IndexCount / 3);
			}
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMouseMoved(MouseState state)
		{
			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = m_viewport.Unproject(new Vector3(state.X, state.Y, 0), RenderingDetails.BasicEffect.Projection, RenderingDetails.BasicEffect.View, RenderingDetails.BasicEffect.World);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = m_viewport.Unproject(new Vector3(state.X, state.Y, 1), RenderingDetails.BasicEffect.Projection, RenderingDetails.BasicEffect.View, RenderingDetails.BasicEffect.World);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Find the grid square containing the nearest terrain triangle hit by the ray (if any).
			Tuple<int,int> pickedGridSquare = m_city.TerrainMesh.PickGridSquare(ray);

			// If we found a grid square, create a temporary building there to show what the user is trying to place.
			if(pickedGridSquare != null)
			{
				m_entityToPlace = BuildingFactory.CreateHouse(pickedGridSquare, EntityOrientation.LEFT2RIGHT, m_city.TerrainMesh);
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			if(state.LeftButton == ButtonState.Pressed)
			{
				if(m_entityToPlace != null && m_entityToPlace.ValidateFootprint())
				{
					m_city.AddEntity((dynamic)m_entityToPlace);
					m_entityToPlace = null;
				}
			}
		}

		#endregion
	}
}
