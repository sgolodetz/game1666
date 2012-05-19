/***
 * game1666: IRetryStrategy.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks.RetryStrategies
{
	/// <summary>
	/// An instance of a class implementing this interface specifies a strategy that
	/// determines the point at which a retryable task should give up.
	/// </summary>
	interface IRetryStrategy
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Alerts the retry strategy that a retry of its task is being attempted at the specified time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void RetryingAt(GameTime gameTime);

		/// <summary>
		/// Decides whether or not the retry strategy's task should try again.
		/// </summary>
		/// <returns>true, if the task should try again, or false otherwise.</returns>
		bool ShouldRetry();

		#endregion
	}
}
