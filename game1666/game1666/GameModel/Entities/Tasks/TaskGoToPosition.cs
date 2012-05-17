/***
 * game1666: TaskGoToPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using game1666.Common.Tasks;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile
	/// entity to head towards a specific position.
	/// </summary>
	sealed class TaskGoToPosition : RetryTask
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' task from a target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		public TaskGoToPosition(Vector2 targetPosition)
		:	base(Int32.MaxValue)
		{
			// TODO
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

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

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Generates a 'follow path' sub-task that does the actual work.
		/// </summary>
		/// <returns>The generated sub-task.</returns>
		protected override Task GenerateSubTask()
		{
			// TODO
			return null;
		}

		#endregion
	}
}
