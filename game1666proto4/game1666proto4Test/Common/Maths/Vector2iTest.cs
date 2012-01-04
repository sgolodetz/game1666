using System;
using game1666proto4.Common.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Maths
{
	[TestClass]
	public class Vector2iTest
	{
		[TestMethod]
		public void op_AdditionTest()
		{
			Vector2i v1 = new Vector2i(23, 9);
			Vector2i v2 = new Vector2i(7, 8);
			Assert.Equal(v1 + v2, new Vector2i(30, 17));
		}

		[TestMethod]
		public void op_DivisionTest()
		{
			Vector2i v = new Vector2i(14, 16);

			// Normal cases.
			Assert.Equal(v / 2, new Vector2i(7, 8));
			Assert.Equal(v / -3, new Vector2i(-4, -5));

			// Case where the divisor is 0.
			Assert.Throws<DivideByZeroException>(() => v / 0);
		}

		[TestMethod]
		public void op_MultiplyTest()
		{
			Vector2i v = new Vector2i(13, 10);
			Vector2i z = new Vector2i(0, 0);

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

		[TestMethod]
		public void op_SubtractionTest()
		{
			Vector2i v1 = new Vector2i(24, 12);
			Vector2i v2 = new Vector2i(17, 10);
			Assert.Equal(v1 - v2, new Vector2i(7, 2));
		}

		[TestMethod]
		public void op_UnaryNegationTest()
		{
			Vector2i v = new Vector2i(21, 4);
			Assert.Equal(-v, new Vector2i(-21, -4));
		}
	}
}
