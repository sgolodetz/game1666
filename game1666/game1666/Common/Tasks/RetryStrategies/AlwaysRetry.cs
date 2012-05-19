/***
 * game1666: AlwaysRetry.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks.RetryStrategies
{
	/// <summary>
	/// An instance of this class represents a retry strategy that specifies that a task should never give up.
	/// </summary>
	sealed class AlwaysRetry : IRetryStrategy
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Alerts the retry strategy that a retry of its task is being attempted at the specified time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void RetryingAt(GameTime gameTime)
		{
			// No-op
		}

		/// <summary>
		/// Decides whether or not the retry strategy's task should try again.
		/// </summary>
		/// <returns>true, if the task should try again, or false otherwise.</returns>
		public bool ShouldRetry()
		{
			return true;
		}

		#endregion
	}
}
