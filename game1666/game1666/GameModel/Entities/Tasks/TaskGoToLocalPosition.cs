/***
 * game1666: TaskGoToLocalPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Tasks;
using game1666.Common.Tasks.RetryStrategies;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity
	/// to head towards a specific position within its containing playing area.
	/// </summary>
	sealed class TaskGoToLocalPosition : RetryableTask
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The target position.
		/// </summary>
		private Vector2 TargetPosition
		{
			get { return Properties["TargetPosition"]; }
			set { Properties["TargetPosition"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to local position' task.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		public TaskGoToLocalPosition(Vector2 targetPosition)
		:	base(new AlwaysRetry())
		{
			TargetPosition = targetPosition;
		}

		/// <summary>
		/// Constructs a 'go to local position' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToLocalPosition(XElement element)
		:	base(new AlwaysRetry(), element)
		{}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Generates a 'follow path' sub-task that does the actual work.
		/// </summary>
		/// <param name="entity">The entity that will execute the sub-task.</param>
		/// <returns>The generated sub-task.</returns>
		protected override Task GenerateSubTask(dynamic entity)
		{
			var mobileComponent = entity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
			var playingAreaComponent = entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

			// Try and find a path to the target position.
			Queue<Vector2> path = playingAreaComponent.NavigationMap.FindPath
			(
				mobileComponent.Position,
				new List<Vector2> { TargetPosition },
				mobileComponent.Properties
			);

			// If a path has been found, return a task that will cause the entity to follow it, else return null.
			return path != null ? new TaskFollowPath(entity, path) : null;
		}

		#endregion
	}
}
