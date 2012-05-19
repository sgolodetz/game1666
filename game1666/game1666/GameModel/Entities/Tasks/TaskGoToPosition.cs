/***
 * game1666: TaskGoToPosition.cs
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
	/// An instance of this class represents a task that causes a mobile entity to head towards a specific position.
	/// </summary>
	sealed class TaskGoToPosition : RetryableTask
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The entity.
		/// </summary>
		private readonly ModelEntity m_entity;

		/// <summary>
		/// The target position.
		/// </summary>
		private readonly Vector2 m_targetPosition;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' task.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="targetPosition">The target position.</param>
		public TaskGoToPosition(ModelEntity entity, Vector2 targetPosition)
		:	base(new AlwaysRetry())
		{
			m_entity = entity;
			m_targetPosition = targetPosition;
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
			var mobileComponent = m_entity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
			var playingAreaComponent = m_entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

			// Try and find a path to the target position.
			Queue<Vector2> path = playingAreaComponent.NavigationMap.FindPath
			(
				mobileComponent.Position,
				new List<Vector2> { m_targetPosition },
				mobileComponent.Properties
			);

			// If a path has been found, return a task that will cause the entity to follow it, else return null.
			return path != null ? new TaskFollowPath(m_entity, path) : null;
		}

		#endregion
	}
}
