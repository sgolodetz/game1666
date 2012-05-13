/***
 * game1666Test: MatchmakingGraphTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666.Common.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test.Common.Matchmaking
{
	/// <summary>
	/// Tests for the MatchmakingGraph class.
	/// </summary>
	[TestClass]
	public sealed class MatchmakingGraphTest
	{
		/// <summary>
		/// A test for the FindBestMatching method.
		/// </summary>
		[TestMethod]
		public void FindBestMatchingTest_1()
		{
			var graph = new MatchmakingGraph(3, 3);
			graph.AddEdge(0, 0, 5);
			graph.AddEdge(0, 1, 5);
			graph.AddEdge(0, 2, 4);
			graph.AddEdge(1, 0, 4);
			graph.AddEdge(1, 1, 6);
			graph.AddEdge(2, 1, 7);
			graph.AddEdge(2, 2, 3);

			graph.FindBestMatching();

			List<MatchmakingEdge> matchingEdges = graph.MatchingEdges.ToList();
			Assert.True(matchingEdges.Count == 3);
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(0, 2, 4)));
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(1, 0, 4)));
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(2, 1, 7)));
		}

		/// <summary>
		/// A test for the FindBestMatching method.
		/// </summary>
		[TestMethod]
		public void FindBestMatchingTest_2()
		{
			var graph = new MatchmakingGraph(5, 5);
			graph.AddEdge(0, 1, 1);
			graph.AddEdge(1, 0, 4);
			graph.AddEdge(1, 2, 3);
			graph.AddEdge(2, 1, 10);
			graph.AddEdge(2, 3, 9);
			graph.AddEdge(3, 2, 6);
			graph.AddEdge(3, 3, 4);
			graph.AddEdge(3, 4, 5);
			graph.AddEdge(4, 3, 3);

			graph.FindBestMatching();

			List<MatchmakingEdge> matchingEdges = graph.MatchingEdges.ToList();
			Assert.True(matchingEdges.Count == 4);
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(1, 0, 4)));
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(2, 1, 10)));
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(3, 2, 6)));
			Assert.True(matchingEdges.Contains(new MatchmakingEdge(4, 3, 3)));
		}
	}
}
