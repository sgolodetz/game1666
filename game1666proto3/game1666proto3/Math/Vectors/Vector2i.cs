/***
 * game1666proto3: Vector2i.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	/// <summary>
	/// Represents a 2D vector with integer components. It is deliberately intended to
	/// mirror the Vector2 class in XNA (which has float components).
	/// </summary>
	struct Vector2i : IEquatable<Vector2i>
	{
		//#################### PUBLIC VARIABLES ####################
		#region

		public int X;	/// the x component of the vector
		public int Y;	/// the y component of the vector

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new vector with the specified components.
		/// </summary>
		/// <param name="x">The x component of the new vector.</param>
		/// <param name="y">The y component of the new vector.</param>
		public Vector2i(int x, int y)
		{
			X = x;
			Y = y;
		}

		#endregion

		//#################### OPERATORS ####################
		#region

		/// <summary>
		/// Returns the result of subtracting one vector from another.
		/// </summary>
		/// <param name="lhs">The left-hand operand of the subtraction.</param>
		/// <param name="rhs">The right-hand operand of the subtraction.</param>
		/// <returns>lhs - rhs</returns>
		public static Vector2i operator-(Vector2i lhs, Vector2i rhs)
		{
			return new Vector2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not this vector is equal to another one.
		/// </summary>
		/// <param name="rhs">The other vector.</param>
		/// <returns>true, if the two vectors are equal, or false otherwise</returns>
		public bool Equals(Vector2i rhs)
		{
			return X == rhs.X && Y == rhs.Y;
		}

		#endregion
	}
}
