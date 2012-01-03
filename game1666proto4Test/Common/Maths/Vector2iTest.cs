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
			Vector2i v1 = new Vector2i(2, 3);
			Vector2i v2 = new Vector2i(5, 7);
			Assert.Equal(v1 + v2, new Vector2i(7, 10));
		}
	}
}
