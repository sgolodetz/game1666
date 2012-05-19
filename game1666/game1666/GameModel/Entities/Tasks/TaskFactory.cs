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
		/// Constructs a task that will make the specified mobile entity go to the specified placeable entity.
		/// </summary>
		/// <param name="entity">The mobile entity.</param>
		/// <param name="targetEntity">Its target placeable entity.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToPlaceableTask(ModelEntity entity, ModelEntity targetEntity)
		{
			return new TaskGoToPlaceable(entity, targetEntity);
		}

		/// <summary>
		/// Constructs a task that will make the specified entity go to the specified target position.
		/// </summary>
		/// <param name="entity">The entity that will move.</param>
		/// <param name="targetPosition">Its target position.</param>
		/// <returns>The constructed task.</returns>
		public Task MakeGoToPositionTask(ModelEntity entity, Vector2 targetPosition)
		{
			return new TaskGoToPosition(entity, targetPosition);
		}

		#endregion
	}
}
