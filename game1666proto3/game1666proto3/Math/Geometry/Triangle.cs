/***
 * game1666proto3: Triangle.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666proto3
{
	/// <summary>
	/// Adds an extension method to Microsoft.Xna.Framework.Ray to support intersection tests with Triangle objects.
	/// </summary>
	static class RayTriangleExtensions
	{
		/// <summary>
		/// Tests whether the ray intersects the specified triangle, and if so, at what distance along its length.
		/// </summary>
		/// <param name="ray">The ray.</param>
		/// <param name="triangle">The triangle.</param>
		/// <returns>The value of the ray's distance parameter at the point at which it hits the triangle (if it does), or null.</returns>
		public static float? Intersects(this Ray ray, Triangle triangle)
		{
			float? distance = ray.Intersects(triangle.DeterminePlane());
			if(distance != null)
			{
				Vector3 p = ray.Position + ray.Direction * distance.Value;
				if(triangle.Contains(p))
				{
					return distance;
				}
			}
			return null;
		}
	}

	/// <summary>
	/// Represents a 3D triangle.
	/// </summary>
	sealed class Triangle
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private Vector3 m_normal;					/// the triangle's normal
		private readonly Vector3[] m_vertices;		/// the triangle's vertices (in anti-clockwise winding order)

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a triangle with the specified three vertices. The three vertices should be
		/// specified in anti-clockwise winding order.
		/// </summary>
		/// <param name="v0">The first vertex.</param>
		/// <param name="v1">The second vertex.</param>
		/// <param name="v2">The third vertex.</param>
		public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			m_vertices = new Vector3[]
			{
				v0, v1, v2
			};

			CalculateNormal();
		}

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public Vector3 Normal		{ get { return m_normal; } }
		public Vector3[] Vertices	{ get { return m_vertices; } }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether the specified point (in the plane of the triangle) is actually within the triangle.
		/// </summary>
		/// <param name="p">The point.</param>
		public bool Contains(Vector3 p)
		{
			// Make sure that the point is in the triangle's plane.
			Plane plane = DeterminePlane();
			if(Vector3.Dot(plane.Normal, p) + plane.D >= Constants.EPSILON) return false;

			// Check that the point is in the triangle itself.
			double angleSum = 0.0;
			for(int i = 0; i < 3; ++i)
			{
				int j = (i + 1) % 3;
				Vector3 a = Vector3.Normalize(m_vertices[i] - p);
				Vector3 b = Vector3.Normalize(m_vertices[j] - p);
				angleSum += Math.Acos(Vector3.Dot(a, b));
			}
			return Math.Abs(angleSum - MathHelper.TwoPi) < Constants.EPSILON;
		}

		/// <summary>
		/// Determines the plane in which the triangle lies.
		/// </summary>
		/// <returns>The plane.</returns>
		public Plane DeterminePlane()
		{
			return new Plane(m_normal, -Vector3.Dot(m_normal, m_vertices[0]));
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Calculates the normal of the triangle.
		/// </summary>
		private void CalculateNormal()
		{
			Vector3 a = m_vertices[1] - m_vertices[0];
			Vector3 b = m_vertices[2] - m_vertices[0];

			m_normal = Vector3.Cross(a, b);

			if(m_normal.LengthSquared() >= Constants.EPSILON)
			{
				m_normal = Vector3.Normalize(m_normal);
			}
			else throw new InvalidOperationException("Cannot calculate the normal of a degenerate triangle.");
		}

		#endregion
	}
}
