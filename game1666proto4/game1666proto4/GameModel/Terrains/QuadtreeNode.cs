/***
 * game1666proto4: QuadtreeNode.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Terrains
{
	/// <summary>
	/// An instance of this class represents a node in a terrain quadtree.
	/// </summary>
	sealed class QuadtreeNode
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// An AABB that bounds the terrain segment represented by this node.
		/// </summary>
		private readonly BoundingBox m_bounds;

		/// <summary>
		/// The children of this node in the quadtree (if any).
		/// </summary>
		private readonly QuadtreeNode[] m_children;

		/// <summary>
		/// The triangles in the terrain segment represented by this node.
		/// </summary>
		private readonly IDictionary<Vector2i,Triangle[]> m_triangleMap;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// An AABB that bounds the terrain segment represented by this node.
		/// </summary>
		public BoundingBox Bounds { get { return m_bounds; } }

		/// <summary>
		/// The children of this node in the quadtree (if any).
		/// </summary>
		public QuadtreeNode[] Children { get { return m_children; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a (branch) quadtree node to contain a set of child quadtree nodes.
		/// </summary>
		/// <param name="children">The child quadtree nodes.</param>
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
		/// Searching the subtree below this quadtree node, find the terrain grid square (if any) hit by the specified ray.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <returns>The nearest terrain grid square hit by the specified ray (if found), or null otherwise.</returns>
		public Vector2i? PickGridSquare(Ray ray)
		{
			if(m_children != null)
			{
				// Find any children whose bounding boxes are hit by the ray, and sort them into
				// non-decreasing order by distance.
				var boundingHits = new SortedDictionary<float,List<QuadtreeNode>>();
				foreach(QuadtreeNode child in m_children)
				{
					float? distance = ray.Intersects(child.m_bounds);
					if(distance != null)
					{
						List<QuadtreeNode> nodes;
						if(boundingHits.ContainsKey(distance.Value))
						{
							nodes = boundingHits[distance.Value];
						}
						else
						{
							nodes = new List<QuadtreeNode>();
							boundingHits.Add(distance.Value, nodes);
						}
						nodes.Add(child);
					}
				}

				// Recursively check each child whose bounding box was hit by the ray in order.
				// If we find a hit, it must be the closest one, so return it.
				foreach(KeyValuePair<float,List<QuadtreeNode>> kv in boundingHits)
				{
					foreach(QuadtreeNode boundingHit in kv.Value)
					{
						Vector2i? result = boundingHit.PickGridSquare(ray);
						if(result != null) return result;
					}
				}

				// If we get here, the ray didn't hit any triangles.
				return null;
			}
			else
			{
				// Find the nearest triangle in this leaf that is hit by the ray (if any).
				float bestDistance = float.MaxValue;
				Vector2i? bestGridSquare = null;
				foreach(KeyValuePair<Vector2i,Triangle[]> triangleSet in m_triangleMap)
				{
					foreach(Triangle triangle in triangleSet.Value)
					{
						float? distance = ray.Intersects(triangle);
						if(distance < bestDistance)
						{
							bestGridSquare = triangleSet.Key;
							bestDistance = distance.Value;
						}
					}
				}
				return bestGridSquare;
			}
		}

		#endregion
	}
}
