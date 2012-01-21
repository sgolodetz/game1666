/***
 * game1666proto4Test: MatchmakingGraphTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Matchmaking;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.Matchmaking
{
	[TestClass]
	public sealed class MatchmakingGraphTest
	{
		[TestMethod]
		public void FindBestMatchingTest()
		{
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
}
