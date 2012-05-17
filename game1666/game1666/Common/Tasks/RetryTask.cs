/***
 * game1666: RetryTask.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
	/// <summary>
	/// An instance of a class deriving from this one represents a task that
	/// tries to succeed a number of times before giving up, regenerating an
	/// inner sub-task as necessary to try and achieve its goal.
	/// </summary>
	abstract class RetryTask : Task
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The current sub-task (will be regenerated as necessary to try and achieve the overall goal).
		/// </summary>
		private Task m_subTask;

		/// <summary>
		/// The number of tries remaining before the task will give up.
		/// </summary>
		private int m_triesLeft;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a retry task that performs the specified number of tries before giving up.
		/// </summary>
		/// <param name="tryCount">The number of tries to perform before giving up.</param>
		protected RetryTask(int tryCount)
		{
			m_triesLeft = tryCount;
		}

		#endregion

		//#################### PROTECTED ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Generates a sub-task that does the actual work.
		/// </summary>
		/// <returns>The generated sub-task.</returns>
		protected abstract Task GenerateSubTask();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(GameTime gameTime)
		{
			if(m_subTask == null)
			{
				if(m_triesLeft > 0)
				{
					// If there's no current sub-task and we haven't used up all of our tries, try and generate a sub-task.
					m_subTask = GenerateSubTask();
					--m_triesLeft;

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
			TaskState result = m_subTask.Execute(gameTime);

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
