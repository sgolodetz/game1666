/***
 * game1666: RetryableTask.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Tasks.RetryStrategies;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
	/// <summary>
	/// An instance of a class deriving from this one represents a task that
	/// tries to succeed multiple times before giving up, regenerating an
	/// inner sub-task as necessary to try and achieve its goal. The point at
	/// which the task gives up is controlled by a retry strategy, which must
	/// be specified by a derived class.
	/// </summary>
	abstract class RetryableTask : Task
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The strategy determining the point at which the task should give up.
		/// </summary>
		private readonly IRetryStrategy m_retryStrategy;

		/// <summary>
		/// The current sub-task (will be regenerated as necessary to try and achieve the overall goal).
		/// </summary>
		private Task m_subTask;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a retryable task that keeps trying until its retry strategy tells it to give up.
		/// </summary>
		/// <param name="retryStrategy">The strategy determining the point at which the task should give up.</param>
		protected RetryableTask(IRetryStrategy retryStrategy)
		{
			m_retryStrategy = retryStrategy;
		}

		/// <summary>
		/// Constructs a retryable task that keeps trying until its retry strategy tells it to give up,
		/// with properties loaded from its XML representation.
		/// </summary>
		/// <param name="retryStrategy">The strategy determining the point at which the task should give up.</param>
		/// <param name="element">The root element of the task's XML representation.</param>
		protected RetryableTask(IRetryStrategy retryStrategy, XElement element)
		:	base(element)
		{
			m_retryStrategy = retryStrategy;
		}

		#endregion

		//#################### PROTECTED ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Generates a sub-task that does the actual work.
		/// </summary>
		/// <param name="entity">The entity that will execute the sub-task.</param>
		/// <returns>The generated sub-task.</returns>
		protected abstract Task GenerateSubTask(dynamic entity);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="entity">The entity that will execute the task.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(dynamic entity, GameTime gameTime)
		{
			if(m_subTask == null)
			{
				if(m_retryStrategy.ShouldRetry())
				{
					// If there's no current sub-task and we haven't used up all of our tries, try and generate a sub-task.
					m_subTask = GenerateSubTask(entity);
					m_retryStrategy.RetryingAt(gameTime);

					// If a sub-task couldn't be generated, early out (we'll try again next time).
					if(m_subTask == null) return TaskState.IN_PROGRESS;
				}
				else
				{
					// If we run out of tries, the retry task fails.
					return TaskState.FAILED;
				}
			}

			// If we get here, a sub-task must have been successfully generated, so execute it.
			TaskState result = m_subTask.Execute(entity, gameTime);

			// If the sub-task fails, clear it - we'll try and regenerate a sub-task next time.
			if(result == TaskState.FAILED)
			{
				m_subTask = null;
				return TaskState.IN_PROGRESS;
			}

			return result;
		}

		#endregion
	}
}
