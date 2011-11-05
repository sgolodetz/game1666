/***
 * game1666proto3: Vector2i.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	struct Vector2i : IEquatable<Vector2i>
	{
		public int X;
		public int Y;

		public Vector2i(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Vector2i operator-(Vector2i lhs, Vector2i rhs)
		{
			return new Vector2i(lhs.X - rhs.X, lhs.Y - rhs.Y);
		}

		public bool Equals(Vector2i rhs)
		{
			return X == rhs.X && Y == rhs.Y;
		}
	}
}
