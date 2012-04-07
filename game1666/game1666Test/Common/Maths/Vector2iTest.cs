/***
 * game1666Test: Vector2iTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666.Common.Maths
{
	/// <summary>
	/// Tests for the Vector2i class.
	/// </summary>
	[TestClass]
	public sealed class Vector2iTest
	{
		/// <summary>
		/// Test the addition operator.
		/// </summary>
		[TestMethod]
		public void op_AdditionTest()
		{
			var v1 = new Vector2i(23, 9);
			var v2 = new Vector2i(7, 8);
			Assert.Equal(v1 + v2, new Vector2i(30, 17));
		}

		/// <summary>
		/// Test the division operator.
		/// </summary>
		[TestMethod]
		public void op_DivisionTest()
		{
			var v = new Vector2i(14, 16);

			// Normal cases.
			Assert.Equal(v / 2, new Vector2i(7, 8));
			Assert.Equal(v / -3, new Vector2i(-4, -5));

			// Case where the divisor is 0.
			Assert.Throws<DivideByZeroException>(() => v / 0);
		}

		/// <summary>
		/// Test the multiplication operator.
		/// </summary>
		[TestMethod]
		public void op_MultiplyTest()
		{
			var v = new Vector2i(13, 10);
			var z = new Vector2i(0, 0);

			// Normal cases.
			Assert.Equal(v * 2, new Vector2i(26, 20));
			Assert.Equal(v * -3, new Vector2i(-39, -30));
			Assert.Equal(v * 4, 4 * v);

			// Cases where the vector is ~0~.
			Assert.Equal(z * 23, new Vector2i(0, 0));
			Assert.Equal(-9 * z, new Vector2i(0, 0));

			// Cases where the scaling factor is 0.
			Assert.Equal(v * 0, new Vector2i(0, 0));
			Assert.Equal(0 * v, new Vector2i(0, 0));
		}

		/// <summary>
		/// Test the subtraction operator.
		/// </summary>
		[TestMethod]
		public void op_SubtractionTest()
		{
			var v1 = new Vector2i(24, 12);
			var v2 = new Vector2i(17, 10);
			Assert.Equal(v1 - v2, new Vector2i(7, 2));
		}

		/// <summary>
		/// Test the unary negation operator.
		/// </summary>
		[TestMethod]
		public void op_UnaryNegationTest()
		{
			var v = new Vector2i(21, 4);
			Assert.Equal(-v, new Vector2i(-21, -4));
		}
	}
}
