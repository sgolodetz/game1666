/***
 * game1666: MathUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace game1666.Common.Maths
{
	/// <summary>
	/// This class provides mathematical utility functions.
	/// </summary>
	static class MathUtil
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Returns a Vector3 with the same x and y components as the input Vector2 and a z component of zero.
		/// </summary>
		/// <param name="v">The input vector (x,y).</param>
		/// <returns>The vector (x,y,0).</returns>
		public static Vector3 FromXY(Vector2 v)
		{
			return new Vector3(v.X, v.Y, 0f);
		}

		/// <summary>
		/// Returns a normalized copy of the specified Vector3.
		/// </summary>
		/// <param name="v">The input vector.</param>
		/// <returns>A normalized copy of the vector.</returns>
		public static Vector3 Normalized(this Vector3 v)
		{
			Vector3 copy = v;
			copy.Normalize();
			return copy;
		}

		/// <summary>
		/// Rotates vector v anticlockwise about the specified axis by the specified angle (in degrees).
		/// </summary>
		/// <param name="v">The vector to rotate about the axis.</param>
		/// <param name="angle">The angle by which to rotate it (in radians).</param>
		/// <param name="axis">The axis about which to rotate it.</param>
		/// <returns>A (new) vector containing the result of the rotation.</returns>
		public static Vector3 RotateAboutAxis(Vector3 v, float angle, Vector3 axis)
		{
			// Check the preconditions.
			Contract.Requires(Math.Abs(axis.Length() - 1) <= Constants.EPSILON);

			// Main algorithm.
			float cosAngle = Convert.ToSingle(Math.Cos(angle)), sinAngle = Convert.ToSingle(Math.Sin(angle));
			Vector3 aCROSSv = Vector3.Cross(axis, v);

			// The rotated vector is v cos radianAngle + (axis x v) sin radianAngle + axis(axis . v)(1 - cos radianAngle)
			// (See Mathematics for 3D Game Programming and Computer Graphics, P.62, for details of why this is (it's not very hard)).
			return v * cosAngle + aCROSSv * sinAngle + axis * Vector3.Dot(axis, v) * (1 - cosAngle);
		}

		/// <summary>
		/// Returns a Vector2i with x and y components that are the result of flooring the components of the input Vector2.
		/// </summary>
		/// <param name="v">The input Vector2 (x,y).</param>
		/// <returns>The Vector2i (floor(x), floor(y)).</returns>
		public static Vector2i ToVector2i(this Vector2 v)
		{
			return new Vector2i((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
		}

		/// <summary>
		/// Returns the x-y portion of the specified Vector3, i.e. slices (x,y,z) -> (x,y).
		/// </summary>
		/// <param name="v">The input vector.</param>
		/// <returns>The x-y portion of the vector.</returns>
		public static Vector2 XY(this Vector3 v)
		{
			return new Vector2(v.X, v.Y);
		}

		#endregion
	}
}
