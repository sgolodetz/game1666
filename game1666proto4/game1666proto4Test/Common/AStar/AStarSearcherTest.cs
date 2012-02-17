/***
 * game1666proto4Test: AStarSearcherTest.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.AStar;
using game1666proto4.Common.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666proto4Test.Common.AStar
{
	[TestClass]
	public sealed class AStarSearcherTest
	{
		//#################### HELPER CLASSES ####################
		#region

		sealed class TestAStarNode : AStarNode<TestAStarNode>
		{
			private TestAStarNode[,] m_grid;
			private Vector2i m_position;

			public override IEnumerable<TestAStarNode> Neighbours
			{
				get
				{
					return NeighbourPositions.Where(pos =>
							0 <= pos.X && pos.X < m_grid.GetLength(1) &&
							0 <= pos.Y && pos.Y < m_grid.GetLength(0) &&
							m_grid[pos.Y, pos.X] != null
						)
						.Select(pos => m_grid[pos.Y, pos.X]);
				}
			}

			private IEnumerable<Vector2i> NeighbourPositions
			{
				get
				{
					int x = m_position.X, y = m_position.Y;
					yield return new Vector2i(x, y-1);
					yield return new Vector2i(x-1, y);
					yield return new Vector2i(x+1, y);
					yield return new Vector2i(x, y+1);
				}
			}

			public TestAStarNode(Vector2i position, TestAStarNode[,] grid)
			{
				m_position = position;
				m_grid = grid;
			}

			public override void CalculateH(ICollection<TestAStarNode> destinations)
			{
				H = destinations.Select(n => CostToNeighbour(n)).Min();
			}

			public override float CostToNeighbour(TestAStarNode neighbour)
			{
				if(m_position.X == 0 && m_position.Y == 1 && neighbour.m_position.X == 1 && neighbour.m_position.Y == 1)
				{
					return 0;
				}
				else
				{
					return Math.Abs(m_position.X - neighbour.m_position.X) + Math.Abs(m_position.Y - neighbour.m_position.Y);
				}
			}
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void FindPathTest()
		{
			var grid = new TestAStarNode[3,3];
			AddNode(0, 0, grid);
			AddNode(0, 1, grid);
			AddNode(1, 0, grid);
			AddNode(1, 1, grid);
			AddNode(2, 0, grid);
			AddNode(2, 1, grid);
			AddNode(2, 2, grid);

			var destinations = new List<TestAStarNode>
			{
				grid[2,2]
			};

			LinkedList<TestAStarNode> path = AStarSearcher<TestAStarNode>.FindPath(grid[0,0], destinations);

			// TODO
		}

		private void AddNode(int x, int y, TestAStarNode[,] grid)
		{
			grid[y,x] = new TestAStarNode(new Vector2i(x,y), grid);
		}

		#endregion
	}
}
