/***
 * game1666: ITaskFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
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
		/// Constructs a task that will make a mobile entity go to the nearest of a set of specified
		/// target positions within its containing playing area.
		/// </summary>
		/// <param name="targetPositions">The target positions.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToALocalPositionTask(IEnumerable<Vector2> targetPositions);

		/// <summary>
		/// Constructs a task that will make a mobile entity go to the specified entity within its
		/// containing playing area.
		/// </summary>
		/// <param name="targetEntity">The absolute path of the target entity.</param>
		/// <returns>The constructed task.</returns>
		Task MakeGoToLocalEntityTask(string targetEntityPath);
	}
}
