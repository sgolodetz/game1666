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

		sealed class TestAStarNode : AStarNode<Vector2i>
		{
			private AStarNode<Vector2i>[,] m_grid;

			public override IEnumerable<AStarNode<Vector2i>> Neighbours
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
					int x = Data.X, y = Data.Y;
					yield return new Vector2i(x, y-1);
					yield return new Vector2i(x-1, y);
					yield return new Vector2i(x+1, y);
					yield return new Vector2i(x, y+1);
				}
			}

			public TestAStarNode(Vector2i position, AStarNode<Vector2i>[,] grid)
			{
				Data = position;
				m_grid = grid;
			}

			public override void CalculateH(ICollection<AStarNode<Vector2i>> destinations)
			{
				H = destinations.Select(n => CostToNeighbour(n)).Min();
			}

			public override float CostToNeighbour(AStarNode<Vector2i> neighbour)
			{
				if(Data.X == 0 && Data.Y == 1 && neighbour.Data.X == 1 && neighbour.Data.Y == 1)
				{
					return 0;
				}
				else
				{
					return Math.Abs(Data.X - neighbour.Data.X) + Math.Abs(Data.Y - neighbour.Data.Y);
				}
			}
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		[TestMethod]
		public void FindPathTest()
		{
			var grid = new AStarNode<Vector2i>[3,3];
			AddNode(0, 0, grid);
			AddNode(0, 1, grid);
			AddNode(1, 0, grid);
			AddNode(1, 1, grid);
			AddNode(2, 0, grid);
			AddNode(2, 1, grid);
			AddNode(2, 2, grid);

			var destinations = new List<AStarNode<Vector2i>>
			{
				grid[2,2]
			};

			LinkedList<AStarNode<Vector2i>> path = AStarSearcher<Vector2i>.FindPath(grid[0,0], destinations);

			// TODO
		}

		private void AddNode(int x, int y, AStarNode<Vector2i>[,] grid)
		{
			grid[y,x] = new TestAStarNode(new Vector2i(x,y), grid);
		}

		#endregion
	}
}
