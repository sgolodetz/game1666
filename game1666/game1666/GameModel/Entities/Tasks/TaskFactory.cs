/***
 * game1666: TaskFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Tasks;
using game1666.GameModel.Entities.Base;
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
		/// Constructs a task that will make a mobile entity go to the specified placeable entity.
		/// </summary>
		/// <param name="targetEntity">The target placeable entity.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToPlaceableTask(ModelEntity targetEntity)
		{
			return new TaskGoToPlaceable(targetEntity);
		}

		/// <summary>
		/// Constructs a task that will make a mobile entity go to the specified target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToPositionTask(Vector2 targetPosition)
		{
			return new TaskGoToPosition(targetPosition);
		}

		#endregion
	}
}
