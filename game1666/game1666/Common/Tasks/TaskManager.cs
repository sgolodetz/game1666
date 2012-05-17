/***
 * game1666: TaskManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
	/// <summary>
	/// An instance of this class manages a priority queue of tasks to be performed.
	/// </summary>
	sealed class TaskManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The task that is currently being performed.
		/// </summary>
		private ITask m_currentTask;

		/// <summary>
		/// Any tasks that are to be performed after the current one finishes.
		/// </summary>
		private readonly SortedDictionary<TaskPriority,LinkedList<ITask>> m_taskQueue = new SortedDictionary<TaskPriority,LinkedList<ITask>>();

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a task to the manager. If there is no current task, the added task becomes the current one.
		/// If there is a current task, the added task gets put on the queue for later execution.
		/// </summary>
		/// <param name="task">The task to add.</param>
		public void AddTask(ITask task)
		{
			if(m_currentTask == null)
			{
				m_currentTask = task;
			}
			else
			{
				LinkedList<ITask> tasks = null;
				if(!m_taskQueue.TryGetValue(task.Priority, out tasks))
				{
					tasks = new LinkedList<ITask>();
					m_taskQueue.Add(task.Priority, tasks);
				}
				tasks.AddLast(task);
			}
		}

		/// <summary>
		/// Executes the current task (if any).
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>true, if there is still a task either in progress or on the queue
		/// after the current task has been executed, or false otherwise.</returns>
		public bool Execute(GameTime gameTime)
		{
			if(m_currentTask == null) return false;
			m_currentTask = m_currentTask.Execute(gameTime) == TaskState.IN_PROGRESS ? m_currentTask : PopNextTask();
			return m_currentTask != null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Pops the next task (if any) off the queue.
		/// </summary>
		/// <returns>The next task (if any), or null otherwise.</returns>
		private ITask PopNextTask()
		{
			foreach(var kv in m_taskQueue)
			{
				LinkedList<ITask> tasks = kv.Value;
				if(tasks.Count != 0)
				{
					ITask task = tasks.First.Value;
					tasks.RemoveFirst();
					return task;
				}
			}

			return null;
		}

		#endregion
	}
}
