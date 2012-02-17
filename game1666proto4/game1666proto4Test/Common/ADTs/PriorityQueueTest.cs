/***
 * game1666proto4Test: PriorityQueueTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.ADTs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.ADTs
{
	[TestClass]
	public sealed class PriorityQueueTest
	{
		//#################### HELPER CLASSES ####################
		#region

		sealed class GreaterComparer<T> : IComparer<T>
		{
			public int Compare(T lhs, T rhs)
			{
				return -Comparer<T>.Default.Compare(lhs, rhs);
			}
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void ClearTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
			pq.Insert("S", 1.0, 23);
				Assert.False(pq.Empty);
			pq.Clear();
				Assert.True(pq.Empty);
		}

		[TestMethod]
		public void ContainsTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
				Assert.False(pq.Contains("S"));
				Assert.False(pq.Contains("K"));
			pq.Insert("S", 1.0, 23);
				Assert.True(pq.Contains("S"));
				Assert.False(pq.Contains("K"));
			pq.Insert("K", 0.9, 13);
				Assert.True(pq.Contains("S"));
				Assert.True(pq.Contains("K"));
		}

		[TestMethod]
		public void CountTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
				Assert.Equal(pq.Count, 0);
			pq.Insert("S", 1.0, 23);
				Assert.Equal(pq.Count, 1);
			pq.Insert("K", 0.9, 13);
				Assert.Equal(pq.Count, 2);
			pq.Insert("M", 1.1, 7);
				Assert.Equal(pq.Count, 3);
			pq.Insert("D", 1.1, 7);
				Assert.Equal(pq.Count, 4);
			pq.Insert("G", 1.1, 24);
				Assert.Equal(pq.Count, 5);
			pq.Erase("S");
				Assert.Equal(pq.Count, 4);
			pq.Insert("S", 1.0, 23);
				Assert.Equal(pq.Count, 5);

			for(int i = 0; i < 5; ++i)
			{
				pq.Pop();
				Assert.Equal(pq.Count, 4 - i);
			}
		}

		[TestMethod]
		public void EraseTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
				Assert.Equal(pq.Count, 0);
			pq.Insert("S", 1.0, 23);
				Assert.Equal(pq.Count, 1);
				Assert.True(pq.Contains("S"));
			pq.Erase("S");
				Assert.Equal(pq.Count, 0);
				Assert.False(pq.Contains("S"));
		}

		[TestMethod]
		public void GetElementTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
			pq.Insert("S", 1.0, 23);
			PriorityQueue<string,double,int>.Element e = pq.GetElement("S");
				Assert.Equal(e.ID, "S");
				Assert.Equal(e.Key, 1.0);
				Assert.Equal(e.Data, 23);
		}

		[TestMethod]
		public void PopTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
			pq.Insert("S", 1.0, 23);
			pq.Insert("K", 0.9, 13);
				Assert.True(pq.Contains("S"));
				Assert.True(pq.Contains("K"));
			pq.Pop();
				Assert.False(pq.Contains("S"));
				Assert.True(pq.Contains("K"));
		}

		[TestMethod]
		public void TopTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
			pq.Insert("S", 1.0, 23);
			pq.Insert("K", 1.1, 13);
				Assert.Equal(pq.Top.ID, "K");
			pq.Pop();
				Assert.Equal(pq.Top.ID, "S");
		}

		[TestMethod]
		public void UpdateKeyTest()
		{
			var pq = new PriorityQueue<string, double, int>(new GreaterComparer<double>());
			pq.Insert("S", 1.0, 23);
			pq.Insert("M", 0.9, 7);
			pq.UpdateKey("M", 1.1);
				Assert.Equal(pq.Top.ID, "M");
				Assert.Equal(pq.Top.Key, 1.1);
				Assert.Equal(pq.Top.Data, 7);
			pq.Pop();
				Assert.Equal(pq.Top.ID, "S");
				Assert.Equal(pq.Top.Key, 1.0);
				Assert.Equal(pq.Top.Data, 23);
			pq.Pop();
				Assert.True(pq.Empty);
		}

		#endregion
	}
}
