/***
 * game1666: TaskGoToPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Tasks;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile
	/// entity to head towards a specific position.
	/// </summary>
	sealed class TaskGoToPosition : Task
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The priority of the task.
		/// </summary>
		private readonly TaskPriority m_priority;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The priority of the task.
		/// </summary>
		public override TaskPriority Priority { get { return m_priority; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' task from a target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		/// <param name="priority">The priority of the task.</param>
		public TaskGoToPosition(Vector2 targetPosition, TaskPriority priority)
		{
			// TODO
			m_priority = priority;
		}

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
			// TODO
			return TaskState.FAILED;
		}

		#endregion
	}
}
