/***
 * game1666proto4: Triangle.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666proto4.Common.Maths
{
	/// <summary>
	/// Adds an extension method to Microsoft.Xna.Framework.Ray to support intersection tests with Triangle objects.
	/// </summary>
	static class RayTriangleExtensions
	{
		//#################### PUBLIC METHODS ####################
		#region

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

		#endregion
	}

	/// <summary>
	/// Represents a 3D triangle.
	/// </summary>
	sealed class Triangle
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The triangle's normal.
		/// </summary>
		public Vector3 Normal		{ get; private set; }

		/// <summary>
		/// The triangle's vertices (in anti-clockwise winding order).
		/// </summary>
		public Vector3[] Vertices	{ get; private set; }

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
			this.Vertices = new Vector3[] { v0, v1, v2 };
			CalculateNormal();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether the specified point (in the plane of the triangle) is actually within the triangle.
		/// </summary>
		/// <param name="p">The point.</param>
		/// <returns>true, if the point is within the triangle, or false otherwise.</returns>
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
				Vector3 a = Vector3.Normalize(this.Vertices[i] - p);
				Vector3 b = Vector3.Normalize(this.Vertices[j] - p);
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
			return new Plane(this.Normal, -Vector3.Dot(this.Normal, this.Vertices[0]));
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Calculates the normal of the triangle.
		/// </summary>
		private void CalculateNormal()
		{
			Vector3 a = this.Vertices[1] - this.Vertices[0];
			Vector3 b = this.Vertices[2] - this.Vertices[0];

			this.Normal = Vector3.Cross(a, b);

			if(this.Normal.LengthSquared() >= Constants.EPSILON)
			{
				this.Normal = Vector3.Normalize(this.Normal);
			}
			else throw new InvalidOperationException("Cannot calculate the normal of a degenerate triangle.");
		}

		#endregion
	}
}
