/***
 * game1666proto4: QuadtreeCompiler.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// This class compiles terrain quadtrees to use for picking.
	/// </summary>
	static class QuadtreeCompiler
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Builds a terrain quadtree from a heightmap, for use in picking.
		/// </summary>
		/// <param name="heightmap">The terrain heightmap.</param>
		/// <returns>The root node of the quadtree.</returns>
		public static QuadtreeNode BuildQuadtree(float[,] heightmap)
		{
			return BuildQuadtreeSub(heightmap, new Vector2i(0, 0), new Vector2i(heightmap.GetLength(1) - 1, heightmap.GetLength(0) - 1));
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Builds a quadtree node from a segment of a heightmap.
		/// </summary>
		/// <param name="heightmap">The heightmap.</param>
		/// <param name="topLeft">The coordinates of the top-left of the segment.</param>
		/// <param name="bottomRight">The coordinates of the bottom-right of the segment.</param>
		/// <returns>The quadtree node for the specified heightmap segment.</returns>
		private static QuadtreeNode BuildQuadtreeSub(float[,] heightmap, Vector2i topLeft, Vector2i bottomRight)
		{
			int segmentWidth = bottomRight.X - topLeft.X;
			int segmentHeight = bottomRight.Y - topLeft.Y;

			if(segmentWidth % 2 == 0 && segmentHeight % 2 == 0)
			{
				// If both the dimensions of the segment are divisible by two, the segment can be further subdivided.
				Vector2i mid = (topLeft + bottomRight) / 2;
				var children = new QuadtreeNode[]
				{
					BuildQuadtreeSub(heightmap, topLeft, mid),
					BuildQuadtreeSub(heightmap, new Vector2i(mid.X, topLeft.Y), new Vector2i(bottomRight.X, mid.Y)),
					BuildQuadtreeSub(heightmap, new Vector2i(topLeft.X, mid.Y), new Vector2i(mid.X, bottomRight.Y)),
					BuildQuadtreeSub(heightmap, mid, bottomRight)
				};
				return new QuadtreeNode(children);
			}
			else
			{
				// Otherwise, construct a leaf node.
				var triangles = new Dictionary<Vector2i,Triangle[]>();
				for(int y = topLeft.Y; y < bottomRight.Y; ++y)
				{
					for(int x = topLeft.X; x < bottomRight.X; ++x)
					{
						var v0 = new Vector3(x, y, heightmap[y,x]) * GameConfig.TERRAIN_SCALE;
						var v1 = new Vector3(x+1, y, heightmap[y,x+1]) * GameConfig.TERRAIN_SCALE;
						var v2 = new Vector3(x, y+1, heightmap[y+1,x]) * GameConfig.TERRAIN_SCALE;
						var v3 = new Vector3(x+1, y+1, heightmap[y+1,x+1]) * GameConfig.TERRAIN_SCALE;

						triangles[new Vector2i(x, y)] = new Triangle[]
						{
							new Triangle(v0, v1, v2),
							new Triangle(v1, v3, v2)
						};
					}
				}
				return new QuadtreeNode(triangles);
			}
		}

		#endregion
	}
}
