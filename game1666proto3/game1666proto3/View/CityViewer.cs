/***
 * game1666proto3: CityViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// A viewer to draw a city on the City screen.
	/// </summary>
	sealed class CityViewer : IViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private City m_city;
		private IndexBuffer m_terrainIndexBuffer;
		private VertexBuffer m_terrainVertexBuffer;
		private readonly Viewport m_viewport;

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
				m_terrainIndexBuffer.Dispose();
				m_terrainVertexBuffer.Dispose();
				m_terrainIndexBuffer = null;
				m_terrainVertexBuffer = null;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="viewport"></param>
		public CityViewer(Viewport viewport)
		{
			m_viewport = viewport;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the city.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">The content manager containing any textures to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect, ContentManager content)
		{
			// If we're not currently viewing a city, don't try and render anything.
			if(m_city == null) return;

			EnsureBuffersCreated(graphics);

			Viewport savedViewport = graphics.Viewport;
			graphics.Viewport = m_viewport;

			// TODO

			graphics.Viewport = savedViewport;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		private void EnsureBuffersCreated(GraphicsDevice graphics)
		{
			// TODO
		}

		#endregion
	}
}
