/***
 * game1666proto4Test: TriangleTest.cs
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
		public void ContainsTest()
		{
			// A triangle in the x = 23 plane.
			var v0 = new Vector3(23,0,0);
			var v1 = new Vector3(23,1,0);
			var v2 = new Vector3(23,0,1);
			var triangle = new Triangle(v0, v1, v2);

			// Double-check the triangle's normal.
			Assert.Equal(triangle.Normal, Vector3.UnitX);

			// Check that the triangle's vertices are classified as being within the triangle.
			Assert.True(triangle.Contains(v0));
			Assert.True(triangle.Contains(v1));
			Assert.True(triangle.Contains(v2));

			// Check that a point on one of the triangle's edges is classified as being within the triangle.
			Assert.True(triangle.Contains(new Vector3(23,0.5f,0)));

			// Check that a point strictly within the triangle is classified as being within the triangle.
			Assert.True(triangle.Contains(new Vector3(23,0.25f,0.25f)));

			// Check that a point outside the triangle, but in the triangle's plane, is classified as being outside the triangle.
			Assert.False(triangle.Contains(new Vector3(23,0.75f,0.75f)));

			// Check that a point not within the triangle's plane is classified as being outside the triangle.
			Assert.False(triangle.Contains(new Vector3(24,0,0)));
		}

		[TestMethod]
		public void ConstructorTest_1()
		{
			// A triangle in the z = 3 plane.
			var v0 = new Vector3(1,2,3);
			var v1 = new Vector3(2,2,3);
			var v2 = new Vector3(2,3,3);
			var triangle = new Triangle(v0, v1, v2);
			Assert.Equal(triangle.Vertices[0], v0);
			Assert.Equal(triangle.Vertices[1], v1);
			Assert.Equal(triangle.Vertices[2], v2);
			Assert.Equal(triangle.Normal, Vector3.UnitZ);
		}

		[TestMethod]
		public void ConstructorTest_2()
		{
			// A triangle in the x + y = 2 plane.
			var v0 = new Vector3(2, 0, 0);
			var v1 = new Vector3(0, 2, 0);
			var v2 = new Vector3(2, 0, 2);
			var triangle = new Triangle(v0, v1, v2);
			Assert.Equal(triangle.Normal, new Vector3(1, 1, 0).Normalized());
		}

		[TestMethod]
		public void ConstructorTest_3()
		{
			// A degenerate triangle.
			var v0 = new Vector3(1, 0, 0);
			var v1 = new Vector3(0, 1, 0);
			Assert.Throws<InvalidOperationException>(() => new Triangle(v0, v1, v0));
		}

		[TestMethod]
		public void DeterminePlaneTest()
		{
			// A triangle in the 4x - 3y - 12 = 0 plane.
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
