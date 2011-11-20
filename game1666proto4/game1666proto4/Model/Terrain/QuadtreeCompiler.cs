/***
 * game1666proto4: QuadtreeCompiler.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

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
			return BuildQuadtreeSub(heightmap, new Vector2i(0, 0), new Vector2i(heightmap.GetLength(1), heightmap.GetLength(0)));
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Builds a quadtree node from a segment of a heightmap.
		/// </summary>
		/// <param name="heightmap">The heightmap.</param>
		/// <param name="topLeft">The coordinates of the top-left of the segment (inclusive).</param>
		/// <param name="bottomRight">The coordinates of the bottom-right of the segment (exclusive).</param>
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
				// TODO
				return new QuadtreeNode(triangles);
			}
		}

		#endregion
	}
}
