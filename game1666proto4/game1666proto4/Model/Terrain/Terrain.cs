/***
 * game1666proto4: Terrain.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	sealed class Terrain
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly bool[,] m_occupancy;
		private readonly QuadtreeNode m_quadtreeRoot;

		#endregion

		//#################### PROPERTIES ####################
		#region

		public IndexBuffer IndexBuffer { get; private set; }
		public VertexBuffer VertexBuffer { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Terrain(float[,] heightmap, float gridSquareWidth, float gridSquareHeight)
		{
			m_occupancy = new bool[heightmap.GetLength(0) - 1, heightmap.GetLength(1) - 1];
			m_quadtreeRoot = QuadtreeCompiler.BuildQuadtree(heightmap, gridSquareWidth, gridSquareHeight);

			ConstructBuffers();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		private void ConstructBuffers()
		{
			// TODO
		}

		#endregion
	}
}
