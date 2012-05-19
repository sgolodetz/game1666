/***
 * game1666: TaskGoToPlaceable.cs
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
	/// An instance of this class represents a task that causes a mobile entity to head towards
	/// a specific placeable entity.
	/// </summary>
	sealed class TaskGoToPlaceable : RetryableTask
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The mobile entity.
		/// </summary>
		private readonly ModelEntity m_entity;

		/// <summary>
		/// The target placeable entity.
		/// </summary>
		private readonly ModelEntity m_targetEntity;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to placeable' task.
		/// </summary>
		/// <param name="entity">The mobile entity.</param>
		/// <param name="targetEntity">The target placeable entity.</param>
		public TaskGoToPlaceable(ModelEntity entity, ModelEntity targetEntity)
		:	base(new AlwaysRetry())
		{
			m_entity = entity;
			m_targetEntity = targetEntity;
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
			var placeableComponent = m_targetEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			var playingAreaComponent = m_entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

			// Try and find a path to the nearest entrance of the target entity.
			List<Vector2> targetEntrances = placeableComponent.Entrances.Select(v => new Vector2(v.X + 0.5f, v.Y + 0.5f)).ToList();
			Queue<Vector2> path = playingAreaComponent.NavigationMap.FindPath
			(
				mobileComponent.Position,
				targetEntrances,
				mobileComponent.Properties
			);

			// If a path has been found, return a task that will cause the entity to follow it, else return null.
			return path != null ? new TaskFollowPath(m_entity, path) : null;
		}

		#endregion
	}
}
