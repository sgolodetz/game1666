/***
 * game1666: TaskGoToAPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Tasks;
using game1666.Common.Tasks.RetryStrategies;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity to
	/// head towards the nearest of a set of specified positions within a containing
	/// entity. Note that this is much more general than a 'go to a local position'
	/// task - it has to plan its way across the world in general, not just the
	/// local playing area.
	/// </summary>
	sealed class TaskGoToAPosition : RetryableTask
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The absolute path of the containing entity.
		/// </summary>
		private string ContainingEntityPath
		{
			get { return Properties["ContainingEntityPath"]; }
			set { Properties["ContainingEntityPath"] = value; }
		}

		/// <summary>
		/// The target positions.
		/// </summary>
		private List<Vector2> TargetPositions
		{
			get { return Properties["TargetPositions"]; }
			set { Properties["TargetPositions"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to a position' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToAPosition(XElement element)
		:	base(new AlwaysRetry(), element)
		{}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Generates a sequence sub-task that does the actual work.
		/// </summary>
		/// <param name="entity">The entity that will execute the sub-task.</param>
		/// <returns>The generated sub-task.</returns>
		protected override Task GenerateSubTask(dynamic entity)
		{
			var result = new SequenceTask();
			result.AddTask(new TaskGoToEntity(ContainingEntityPath, new NeverRetry()));
			result.AddTask(new TaskGoToALocalPosition(TargetPositions, new NeverRetry()));
			return result;
		}

		#endregion
	}
}
