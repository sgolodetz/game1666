/***
 * game1666: TaskFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Interfaces.Context;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class can be used to construct tasks.
	/// </summary>
	sealed class TaskFactory : ITaskFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a task that will make a mobile entity go to the nearest of a set of specified
		/// target positions within its containing playing area.
		/// </summary>
		/// <param name="targetPositions">The target positions.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToALocalPositionTask(IEnumerable<Vector2> targetPositions)
		{
			return new TaskGoToALocalPosition(targetPositions);
		}

		/// <summary>
		/// Constructs a task that will make a mobile entity go to the specified placeable entity
		/// within its containing playing area.
		/// </summary>
		/// <param name="targetEntity">The absolute path of the target placeable entity.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToLocalPlaceableTask(string targetEntityPath)
		{
			return new TaskGoToLocalPlaceable(targetEntityPath);
		}

		#endregion
	}
}
