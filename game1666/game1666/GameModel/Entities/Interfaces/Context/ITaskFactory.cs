/***
 * game1666: ITaskFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Tasks;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Interfaces.Context
{
	/// <summary>
	/// An instance of a class implementing this interface can be used to construct tasks.
	/// </summary>
	interface ITaskFactory
	{
		/// <summary>
		/// Constructs a task that will make a mobile entity go to the specified target position
		/// within its containing playing area.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToLocalPositionTask(Vector2 targetPosition);

		/// <summary>
		/// Constructs a task that will make a mobile entity go to the specified placeable entity.
		/// </summary>
		/// <param name="targetEntity">The absolute path of the target placeable entity.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToPlaceableTask(string targetEntityPath);
	}
}
