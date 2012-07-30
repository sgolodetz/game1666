/***
 * game1666: SequenceTask.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
	/// <summary>
	/// An instance of this class represents a composite task that contains
	/// a sequence of tasks to be executed. These tasks will be executed in
	/// order; if any task fails, the sequence as a whole does as well.
	/// </summary>
	sealed class SequenceTask : Task
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The remaining tasks to be executed.
		/// </summary>
		private readonly LinkedList<Task> m_tasks = new LinkedList<Task>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sequence task that initially has no sub-tasks.
		/// </summary>
		public SequenceTask()
		{}

		/// <summary>
		/// Constructs a sequence task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public SequenceTask(XElement element)
		{
			m_tasks = new LinkedList<Task>(ObjectPersister.LoadChildObjects<Task>(element));
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a task to the end of the sequence.
		/// </summary>
		/// <param name="task">The task to add.</param>
		public void AddTask(Task task)
		{
			m_tasks.AddLast(task);
		}

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="entity">The entity that will execute the task.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(dynamic entity, GameTime gameTime)
		{
			if(m_tasks.Count == 0) return TaskState.SUCCEEDED;

			TaskState result = m_tasks.First.Value.Execute(entity, gameTime);
			if(result == TaskState.SUCCEEDED)
			{
				m_tasks.RemoveFirst();
				return m_tasks.Count > 0 ? TaskState.IN_PROGRESS : TaskState.SUCCEEDED;
			}
			else return result;
		}

		/// <summary>
		/// Saves the task to XML.
		/// </summary>
		/// <returns>An XML representation of the task.</returns>
		public override XElement SaveToXML()
		{
			XElement element = ObjectPersister.ConstructObjectElement(GetType());
			ObjectPersister.SaveChildObjects(element, m_tasks);
			return element;
		}

		#endregion
	}
}
