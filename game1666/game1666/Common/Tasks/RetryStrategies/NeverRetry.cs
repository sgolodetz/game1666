/***
 * game1666: NeverRetry.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks.RetryStrategies
{
	/// <summary>
	/// An instance of this class represents a retry strategy that specifies that a task
	/// should give up after the first failure.
	/// </summary>
	sealed class NeverRetry : IRetryStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Whether or not the task has already tried to succeed.
		/// </summary>
		private bool m_tried = false;

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Alerts the retry strategy that a retry of its task is being attempted at the specified time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void RetryingAt(GameTime gameTime)
		{
			m_tried = true;
		}

		/// <summary>
		/// Decides whether or not the retry strategy's task should try again.
		/// </summary>
		/// <returns>true, if the task should try again, or false otherwise.</returns>
		public bool ShouldRetry()
		{
			return !m_tried;
		}

		#endregion
	}
}
