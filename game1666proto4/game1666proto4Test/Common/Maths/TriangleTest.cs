/***
 * game1666proto4: TriangleTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Maths
{
	[TestClass]
	public sealed class TriangleTest
	{
		[TestMethod]
		public void ConstructorTest()
		{
			// A simple triangle in the z = 3 plane
			{
			var v0 = new Vector3(1,2,3);
			var v1 = new Vector3(2,2,3);
			var v2 = new Vector3(2,3,3);
			var triangle = new Triangle(v0, v1, v2);
			Assert.Equal(triangle.Vertices[0], v0);
			Assert.Equal(triangle.Vertices[1], v1);
			Assert.Equal(triangle.Vertices[2], v2);
			Assert.Equal(triangle.Normal, Vector3.UnitZ);
			}
		}
	}
}
