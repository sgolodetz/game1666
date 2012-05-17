/***
 * game1666: Task.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
	/// <summary>
	/// The different possible priorities for a task.
	/// </summary>
	enum TaskPriority
	{
		VERY_HIGH,
		HIGH,
		MEDIUM,
		LOW
	}

	/// <summary>
	/// The different possible states in which a task can be.
	/// </summary>
	enum TaskState
	{
		FAILED,			// the task could not be completed
		IN_PROGRESS,	// the task is still in progress
		SUCCEEDED,		// the task has finished successfully
	}

	/// <summary>
	/// An instance of a class deriving from this one represents a task to be performed.
	/// </summary>
	abstract class Task : IEquatable<Task>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The priority of the task.
		/// </summary>
		public abstract TaskPriority Priority { get; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public abstract TaskState Execute(GameTime gameTime);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not this task is equal to another one.
		/// </summary>
		/// <param name="rhs">The other task.</param>
		/// <returns>true, if the two tasks are equal, or false otherwise.</returns>
		public bool Equals(Task rhs)
		{
			return object.ReferenceEquals(this, rhs);
		}

		#endregion
	}
}
