/***
 * game1666proto4: MathUtil.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace game1666proto4.Common.Maths
{
	/// <summary>
	/// This class provides mathematical utility functions.
	/// </summary>
	static class MathUtil
	{
		//#################### PUBLIC METHODS ####################
		#region

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

		#endregion
	}
}
