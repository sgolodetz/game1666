/***
 * game1666proto4: TriangleTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Maths
{
	[TestClass]
	public sealed class TriangleTest
	{
		[TestMethod]
		public void ConstructorTest()
		{
			// A triangle in the z = 3 plane
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

			// A triangle in the x + y = 2 plane
			{
				var v0 = new Vector3(2, 0, 0);
				var v1 = new Vector3(0, 2, 0);
				var v2 = new Vector3(2, 0, 2);
				var triangle = new Triangle(v0, v1, v2);
				var desiredNormal = new Vector3(1, 1, 0);
				desiredNormal.Normalize();
				Assert.Equal(triangle.Normal, desiredNormal);
			}

			// A degenerate triangle
			{
				var v0 = new Vector3(1, 0, 0);
				var v1 = new Vector3(0, 1, 0);
				Assert.Throws<InvalidOperationException>(() => new Triangle(v0, v1, v0));
			}
		}

		[TestMethod]
		public void DeterminePlaneTest()
		{
			// A triangle in the 4x - 3y - 12 = 0 plane
			{
				var v0 = new Vector3(0, -4, 0);
				var v1 = new Vector3(3, 0, 0);
				var v2 = new Vector3(0, -4, 23);
				Plane plane = new Triangle(v0, v1, v2).DeterminePlane();
				var desiredNormal = new Vector3(4, -3, 0);
				Assert.Equal(plane.D, -12 / desiredNormal.Length());
				desiredNormal.Normalize();
				Assert.Equal(plane.Normal, desiredNormal);
			}
		}
	}
}
