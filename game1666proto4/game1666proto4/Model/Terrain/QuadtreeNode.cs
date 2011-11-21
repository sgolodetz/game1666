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

		private readonly BoundingBox m_bounds;								/// an AABB that bounds the terrain segment represented by this node
		private readonly QuadtreeNode[] m_children;							/// the children of this node in the quadtree (if any)
		private readonly IDictionary<Vector2i,Triangle[]> m_triangleMap;	/// the triangles in the terrain segment represented by this node

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

			// Calculate the tightest axis-aligned bounding box that surrounds all the children of this quadtree node.
			var mins = new Vector3(float.MaxValue);
			var maxs = new Vector3(float.MinValue);
			foreach(QuadtreeNode child in children)
			{
				mins.X = Math.Min(mins.X, child.m_bounds.Min.X);
				mins.Y = Math.Min(mins.Y, child.m_bounds.Min.Y);
				mins.Z = Math.Min(mins.Z, child.m_bounds.Min.Z);
				maxs.X = Math.Max(maxs.X, child.m_bounds.Max.X);
				maxs.Y = Math.Max(maxs.Y, child.m_bounds.Max.Y);
				maxs.Z = Math.Max(maxs.Z, child.m_bounds.Max.Z);
			}
			m_bounds = new BoundingBox(mins, maxs);
		}

		/// <summary>
		/// Constructs a (leaf) quadtree node to contain a set of triangles.
		/// </summary>
		/// <param name="triangleMap">The triangles, grouped by grid square.</param>
		public QuadtreeNode(IDictionary<Vector2i,Triangle[]> triangleMap)
		{
			m_triangleMap = triangleMap;

			// Calculate the tightest axis-aligned bounding box that surrounds all the triangles in this quadtree node.
			var mins = new Vector3(float.MaxValue);
			var maxs = new Vector3(float.MinValue);
			foreach(KeyValuePair<Vector2i,Triangle[]> gridSquare in triangleMap)
			{
				foreach(Triangle triangle in gridSquare.Value)
				{
					foreach(Vector3 v in triangle.Vertices)
					{
						mins.X = Math.Min(mins.X, v.X);
						mins.Y = Math.Min(mins.Y, v.Y);
						mins.Z = Math.Min(mins.Z, v.Z);
						maxs.X = Math.Max(maxs.X, v.X);
						maxs.Y = Math.Max(maxs.Y, v.Y);
						maxs.Z = Math.Max(maxs.Z, v.Z);
					}
				}
			}
			m_bounds = new BoundingBox(mins, maxs);
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
