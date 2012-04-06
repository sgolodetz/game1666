/***
 * game1666: Vector2i.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;

namespace game1666.Common.Maths
{
	/// <summary>
	/// An instance of this struct represents a 2D vector with integer components.
	/// The struct is deliberately intended to mirror Vector2 in XNA (which has float components).
	/// </summary>
	struct Vector2i : IEquatable<Vector2i>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly int m_x;
		private readonly int m_y;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The x component of the vector.
		/// </summary>
		public int X { get { return m_x; } }

		/// <summary>
		/// The y component of the vector.
		/// </summary>
		public int Y { get { return m_y; } }

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
			m_x = x;
			m_y = y;
		}

		#endregion

		//#################### OPERATORS ####################
		#region

		/// <summary>
		/// Returns the result of adding one vector to another.
		/// </summary>
		/// <param name="lhs">The left-hand operand of the addition.</param>
		/// <param name="rhs">The right-hand operand of the addition.</param>
		/// <returns>The result of lhs + rhs.</returns>
		public static Vector2i operator+(Vector2i lhs, Vector2i rhs)
		{
			return new Vector2i(lhs.X + rhs.X, lhs.Y + rhs.Y);
		}

		/// <summary>
		/// Returns the negation of a vector.
		/// </summary>
		/// <param name="v">The vector.</param>
		/// <returns>The result of -v.</returns>
		public static Vector2i operator-(Vector2i v)
		{
			return new Vector2i(-v.X, -v.Y);
		}

		/// <summary>
		/// Returns the result of subtracting one vector from another.
		/// </summary>
		/// <param name="lhs">The left-hand operand of the subtraction.</param>
		/// <param name="rhs">The right-hand operand of the subtraction.</param>
		/// <returns>The result of lhs - rhs.</returns>
		public static Vector2i operator-(Vector2i lhs, Vector2i rhs)
		{
			return new Vector2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		/// <summary>
		/// Returns the result of scaling a vector by a factor.
		/// </summary>
		/// <param name="scaleFactor">The factor by which to scale the vector.</param>
		/// <param name="v">The vector.</param>
		/// <returns>The result of v * scaleFactor.</returns>
		public static Vector2i operator*(int scaleFactor, Vector2i v)
		{
			return new Vector2i(v.X * scaleFactor, v.Y * scaleFactor);
		}

		/// <summary>
		/// Returns the result of scaling a vector by a factor.
		/// </summary>
		/// <param name="v">The vector.</param>
		/// <param name="scaleFactor">The factor by which to scale the vector.</param>
		/// <returns>The result of v * scaleFactor.</returns>
		public static Vector2i operator*(Vector2i v, int scaleFactor)
		{
			return new Vector2i(v.X * scaleFactor, v.Y * scaleFactor);
		}

		/// <summary>
		/// Returns the result of dividing a vector by a factor using integer division.
		/// </summary>
		/// <param name="v">The vector.</param>
		/// <param name="divideFactor">The factor by which to divide the vector.</param>
		/// <returns>The result of v `div` divideFactor.</returns>
		public static Vector2i operator/(Vector2i v, int divideFactor)
		{
			return new Vector2i(v.X / divideFactor, v.Y / divideFactor);
		}

		/// <summary>
		/// Tests whether or not two vectors are equal.
		/// </summary>
		/// <param name="lhs">The left-hand operand of the comparison.</param>
		/// <param name="rhs">The right-hand operand of the comparison.</param>
		/// <returns>true, if the two vectors are equal, or false otherwise.</returns>
		public static bool operator==(Vector2i lhs, Vector2i rhs)
		{
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Tests whether or not two vectors are unequal.
		/// </summary>
		/// <param name="lhs">The left-hand operand of the comparison.</param>
		/// <param name="rhs">The right-hand operand of the comparison.</param>
		/// <returns>true, if the two vectors are unequal, or false otherwise.</returns>
		public static bool operator!=(Vector2i lhs, Vector2i rhs)
		{
			return !lhs.Equals(rhs);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not this vector is equal to another object.
		/// </summary>
		/// <param name="rhs">The other object.</param>
		/// <returns>true, if the other object is a vector equal to this one, or false otherwise.</returns>
		public override bool Equals(object rhs)
		{
			if(rhs is Vector2i) return Equals((Vector2i)rhs);
			else return false;
		}

		/// <summary>
		/// Tests whether or not this vector is equal to another one.
		/// </summary>
		/// <param name="rhs">The other vector.</param>
		/// <returns>true, if the two vectors are equal, or false otherwise.</returns>
		public bool Equals(Vector2i rhs)
		{
			return X == rhs.X && Y == rhs.Y;
		}

		/// <summary>
		/// Returns the hash code for this vector.
		/// </summary>
		/// <returns>The hash code for this vector.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>
		/// Returns the length of the vector.
		/// </summary>
		/// <returns>The length of the vector.</returns>
		public float Length()
		{
			return (float)Math.Sqrt(m_x * m_x + m_y * m_y);
		}

		/// <summary>
		/// Returns a string representation of this vector.
		/// </summary>
		/// <returns>A string representation of this vector.</returns>
		public override string ToString()
		{
			return m_x.ToString() + "," + m_y.ToString();
		}

		#endregion
	}
}
