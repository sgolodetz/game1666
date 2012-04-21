/***
 * game1666Test: AStarSearcherTest.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666.Common.AStar;
using game1666.Common.Maths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Xunit.Assert;

namespace game1666Test.Common.AStar
{
	/// <summary>
	/// Tests for the AStarSearcher class.
	/// </summary>
	[TestClass]
	public sealed class AStarSearcherTest
	{
		//#################### HELPER CLASSES ####################
		#region

		sealed class TestAStarNode : AStarNode<TestAStarNode>
		{
			private TestAStarNode[,] m_grid;

			public Vector2i Position { get; private set; }

			private IEnumerable<Vector2i> NeighbourPositions
			{
				get
				{
					int x = Position.X, y = Position.Y;
					yield return new Vector2i(x-1, y-1);
					yield return new Vector2i(x, y-1);
					yield return new Vector2i(x+1, y-1);
					yield return new Vector2i(x-1, y);
					yield return new Vector2i(x+1, y);
					yield return new Vector2i(x-1, y+1);
					yield return new Vector2i(x, y+1);
					yield return new Vector2i(x+1, y+1);
				}
			}

			public TestAStarNode(Vector2i position, TestAStarNode[,] grid)
			{
				Position = position;
				m_grid = grid;
			}

			public override void CalculateH(ICollection<TestAStarNode> destinations)
			{
				H = destinations.Select(n => CostToNeighbour(n)).Min();
			}

			public override float CostToNeighbour(TestAStarNode neighbour)
			{
				return (Position - neighbour.Position).Length();
			}

			public override IEnumerable<TestAStarNode> Neighbours(IDictionary<string,dynamic> properties)
			{
				return NeighbourPositions.Where(pos =>
					0 <= pos.X && pos.X < m_grid.GetLength(1) &&
					0 <= pos.Y && pos.Y < m_grid.GetLength(0) &&
					m_grid[pos.Y, pos.X] != null
				)
				.Select(pos => m_grid[pos.Y, pos.X]);
			}
		}

		#endregion

		//#################### TEST METHODS ####################
		#region

		/// <summary>
		/// Test the FindPath method.
		/// </summary>
		[TestMethod]
		public void FindPathTest()
		{
			// Construct a simple 3x3 grid with a few nodes missing.
			var grid = new TestAStarNode[3,3];
			AddNode(0, 0, grid);
			AddNode(0, 1, grid);
			AddNode(1, 0, grid);
			AddNode(2, 0, grid);
			AddNode(2, 1, grid);
			AddNode(2, 2, grid);

			// Specify the destination node (2,2).
			var destinations = new List<TestAStarNode> { grid[2,2] };

			// Find the path.
			List<TestAStarNode> path = AStarSearcher<TestAStarNode>.FindPath(grid[0,0], destinations, null).ToList();

			// Check that it's what we expect, namely -> (1,0) -> (2,1) -> (2,2).
			Assert.Equal(path.Count, 3);
			Assert.Equal(path[0].Position, new Vector2i(1,0));
			Assert.Equal(path[1].Position, new Vector2i(2,1));
			Assert.Equal(path[2].Position, new Vector2i(2,2));
		}

		#endregion

		//#################### HELPER METHODS ####################
		#region

		private void AddNode(int x, int y, TestAStarNode[,] grid)
		{
			grid[y,x] = new TestAStarNode(new Vector2i(x,y), grid);
		}

		#endregion
	}
}
