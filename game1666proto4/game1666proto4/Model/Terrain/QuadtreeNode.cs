/***
 * game1666proto4: QuadtreeNode.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a node in a terrain quadtree.
	/// </summary>
	sealed class QuadtreeNode
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly BoundingBox m_bounds;							/// an AABB that bounds the terrain segment represented by this node
		private readonly QuadtreeNode[] m_children;						/// the children of this node in the quadtree (if any)
		private readonly IDictionary<Vector2i,Triangle[]> m_triangles;	/// the triangles in the terrain segment represented by this node

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a (branch) quadtree node to contain a set of child quadtree nodes.
		/// </summary>
		/// <param name="children"></param>
		public QuadtreeNode(QuadtreeNode[] children)
		{
			m_children = children;
			// TODO: Calculate bounding box.
		}

		/// <summary>
		/// Constructs a (leaf) quadtree node to contain a set of triangles.
		/// </summary>
		/// <param name="triangles">The triangles, grouped by grid square.</param>
		public QuadtreeNode(IDictionary<Vector2i,Triangle[]> triangles)
		{
			m_triangles = triangles;
			// TODO: Calculate bounding box.
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Finds the terrain grid square (if any) hit by the specified ray.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <returns>The nearest terrain grid square hit by the specified ray (if found), or null otherwise.</returns>
		public Vector2i? PickGridSquare(Ray ray)
		{
			// TODO
			return null;
		}

		#endregion
	}
}
