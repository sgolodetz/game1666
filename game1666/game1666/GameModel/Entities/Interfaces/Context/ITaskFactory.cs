﻿/***
 * game1666: ITaskFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Tasks;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Interfaces.Context
{
	/// <summary>
	/// An instance of a class implementing this interface can be used to construct tasks.
	/// </summary>
	interface ITaskFactory
	{
		/// <summary>
		/// Constructs a task that will make the specified mobile entity go to the specified placeable entity.
		/// </summary>
		/// <param name="entity">The mobile entity.</param>
		/// <param name="targetEntity">Its target placeable entity.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToPlaceableTask(ModelEntity entity, ModelEntity targetEntity);

		/// <summary>
		/// Constructs a task that will make the specified mobile entity go to the specified target position.
		/// </summary>
		/// <param name="entity">The mobile entity.</param>
		/// <param name="targetPosition">Its target position.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToPositionTask(ModelEntity entity, Vector2 targetPosition);
	}
}
