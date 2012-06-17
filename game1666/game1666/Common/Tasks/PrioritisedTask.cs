/***
 * game1666: PrioritirisedTask.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;

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
	/// An instance of this class represents a task with an assigned priority.
	/// </summary>
	sealed class PrioritisedTask : IPersistableObject
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The priority of the task.
		/// </summary>
		public TaskPriority Priority { get; private set; }

		/// <summary>
		/// The task itself.
		/// </summary>
		public Task Task { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a prioritirised task.
		/// </summary>
		/// <param name="task">The task itself.</param>
		/// <param name="priority">The priority of the task.</param>
		public PrioritisedTask(Task task, TaskPriority priority)
		{
			Task = task;
			Priority = priority;
		}

		/// <summary>
		/// Constructs a prioritirised task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public PrioritisedTask(XElement element)
		{
			Task = ObjectPersister.LoadChildObjects<Task>(element).First();
			Priority = Enum.Parse(typeof(TaskPriority), PropertyPersister.LoadProperties(element)["Priority"], true);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the prioritirised task to XML.
		/// </summary>
		/// <returns>An XML representation of the task.</returns>
		public XElement SaveToXML()
		{
			XElement element = ObjectPersister.ConstructObjectElement(GetType());
			PropertyPersister.SaveProperties(element, new Dictionary<string,dynamic>
			{
				{ "Priority", Priority.ToString() }
			});
			ObjectPersister.SaveChildObjects(element, new List<IPersistableObject> { Task });
			return element;
		}

		#endregion
	}
}
