/***
 * game1666: TaskGoToALocalPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
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
	/// to head towards the nearest of a set of specified positions within its
	/// containing playing area.
	/// </summary>
	sealed class TaskGoToALocalPosition : RetryableTask
	{
		//#################### PROPERTIES ####################
		#region

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
		/// Constructs a 'go to a local position' task.
		/// </summary>
		/// <param name="targetPosition">The target positions.</param>
		public TaskGoToALocalPosition(IEnumerable<Vector2> targetPositions)
		:	this(targetPositions, new AlwaysRetry())
		{}

		/// <summary>
		/// Constructs a 'go to a local position' task with the specified retry strategy.
		/// </summary>
		/// <param name="targetPosition">The target positions.</param>
		/// <param name="retryStrategy">The strategy determining the point at which the task should give up.</param>
		public TaskGoToALocalPosition(IEnumerable<Vector2> targetPositions, IRetryStrategy retryStrategy)
		:	base(retryStrategy)
		{
			TargetPositions = targetPositions.ToList();
		}

		/// <summary>
		/// Constructs a 'go to a local position' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToALocalPosition(XElement element)
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

			// Try and find a path to the nearest target position.
			Queue<Vector2> path = playingAreaComponent.NavigationMap.FindPath
			(
				mobileComponent.Position,
				TargetPositions,
				mobileComponent.Properties
			);

			// If a path has been found, return a task that will cause the entity to follow it, else return null.
			return path != null ? new TaskFollowPath(entity, path) : null;
		}

		#endregion
	}
}
