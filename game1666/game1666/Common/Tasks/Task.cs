/***
 * game1666: Task.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Tasks
{
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
	abstract class Task : IPersistableObject, IEquatable<Task>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the task.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; private set; }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="entity">The entity that will execute the task.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public abstract TaskState Execute(dynamic entity, GameTime gameTime);

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a task.
		/// </summary>
		protected Task()
		{
			Properties = new Dictionary<string,dynamic>();
		}

		/// <summary>
		/// Constructs a task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		protected Task(XElement element)
		{
			Properties = PropertyPersister.LoadProperties(element);
		}

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

		/// <summary>
		/// Saves the task to XML.
		/// </summary>
		/// <returns>An XML representation of the task.</returns>
		public virtual XElement SaveToXML()
		{
			XElement element = ObjectPersister.ConstructObjectElement(GetType());
			PropertyPersister.SaveProperties(element, Properties);
			return element;
		}

		#endregion
	}
}
