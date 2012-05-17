/***
 * game1666: PriorityQueueTask.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.ADTs;
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
	/// An instance of this class represents a composite task that contains a priority queue.
	/// Sub-tasks will be executed in order of priority, with failed tasks being discarded.
	/// Once a sub-task has started executing, it will be allowed to run to completion (in
	/// other words, there is no task preemption implemented).
	/// </summary>
	sealed class PriorityQueueTask : Task
	{
		//#################### PRIVATE VARIABLES ####################

		/// <summary>
		/// The current task being performed.
		/// </summary>
		private Task m_currentTask;

		/// <summary>
		/// The queue of other tasks to be performed.
		/// </summary>
		private readonly PriorityQueue<Task,TaskPriority, int> m_taskQueue = new PriorityQueue<Task,TaskPriority,int>(Comparer<TaskPriority>.Default);

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a task to the queue.
		/// </summary>
		/// <param name="task">The task to add.</param>
		/// <param name="priority">The priority of the task.</param>
		public void AddTask(Task task, TaskPriority priority)
		{
			const int dummy = -1;
			m_taskQueue.Insert(task, priority, dummy);
		}

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(GameTime gameTime)
		{
			if(TrySetCurrentTask())
			{
				TaskState result = m_currentTask.Execute(gameTime);
				if(result == TaskState.IN_PROGRESS)
				{
					return TaskState.IN_PROGRESS;
				}
				else
				{
					m_currentTask = null;
					if(TrySetCurrentTask()) return TaskState.IN_PROGRESS;
				}
			}

			return TaskState.SUCCEEDED;
		}

		/// <summary>
		/// Saves the task to XML.
		/// </summary>
		/// <returns>An XML representation of the task.</returns>
		public override XElement SaveToXML()
		{
			// TODO
			return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Tries to set a current task to be executed.
		/// </summary>
		/// <returns>true, if a current task was set, or false otherwise.</returns>
		private bool TrySetCurrentTask()
		{
			if(m_currentTask != null) return true;
			if(m_taskQueue.Empty) return false;

			m_currentTask = m_taskQueue.Top.ID;
			m_taskQueue.Pop();
			return true;
		}

		#endregion
	}
}
