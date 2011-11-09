/***
 * game1666proto3: CityViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly City m_city;								/// the city being viewed
		private EntityOrientation m_entityPlacementOrientation;		/// the desired orientation of the entity to be placed (if any)
		private PlaceableModelEntity m_entityToPlace;				/// the entity currently being placed by the user (if any)
		private Vector2i? m_pickedGridSquare;						/// the coordinates of the grid square over which the user is currently hovering (if any)
		private readonly Viewport m_viewport;						/// the viewport into which the city will be drawn

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

			// The default entity placement orientation is left-to-right, but the user can change this.
			m_entityPlacementOrientation = EntityOrientation.LEFT2RIGHT;

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
				DrawEntity(entity as dynamic);
			}

			if(m_entityToPlace != null)
			{
				DrawEntity(m_entityToPlace as dynamic);
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
			BasicEffect basicEffect = RenderingDetails.BasicEffect.Clone() as BasicEffect;
			basicEffect.VertexColorEnabled = true;
			DrawTriangleList(building.VertexBuffer, building.IndexBuffer, basicEffect);
		}

		/// <summary>
		/// Draws the terrain on which the city is founded.
		/// </summary>
		private void DrawTerrain()
		{
			BasicEffect basicEffect = RenderingDetails.BasicEffect.Clone() as BasicEffect;
			basicEffect.Texture = RenderingDetails.Content.Load<Texture2D>("landscape");
			basicEffect.TextureEnabled = true;
			DrawTriangleList(m_city.TerrainMesh.VertexBuffer, m_city.TerrainMesh.IndexBuffer, basicEffect);
		}

		/// <summary>
		/// Draws a triangle list using vertices and indices from the specified buffers.
		/// </summary>
		/// <param name="vertexBuffer">The vertex buffer containing the triangles' vertices.</param>
		/// <param name="indexBuffer">The index buffer specifying how the vertices make up the triangles.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		private static void DrawTriangleList(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, BasicEffect basicEffect)
		{
			RenderingDetails.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			RenderingDetails.GraphicsDevice.Indices = indexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				RenderingDetails.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
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
			m_pickedGridSquare = m_city.TerrainMesh.PickGridSquare(ray);

			// If we found a grid square, create a temporary building there to show what the user is trying to place.
			if(m_pickedGridSquare != null)
			{
				m_entityToPlace = BuildingFactory.CreateHouse(m_pickedGridSquare.Value, m_entityPlacementOrientation, m_city.TerrainMesh);
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
				if(m_entityToPlace != null && m_entityToPlace.CanPlace)
				{
					m_city.AddEntity(m_entityToPlace as dynamic);
					m_entityToPlace = null;
				}
			}
			else if(state.RightButton == ButtonState.Pressed)
			{
				int entityOrientationCount = (int)Enum.GetValues(typeof(EntityOrientation)).Cast<EntityOrientation>().Last() + 1;
				m_entityPlacementOrientation = (EntityOrientation)((int)(m_entityPlacementOrientation + 1) % entityOrientationCount);

				if(m_pickedGridSquare != null)
				{
					m_entityToPlace = BuildingFactory.CreateHouse(m_pickedGridSquare.Value, m_entityPlacementOrientation, m_city.TerrainMesh);
				}
			}
		}

		#endregion
	}
}
