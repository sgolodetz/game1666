/***
 * game1666: TaskGoToPlaceable.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
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
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The absolute path of the target placeable entity.
		/// </summary>
		private string TargetEntityPath
		{
			get { return Properties["TargetEntityPath"]; }
			set { Properties["TargetEntityPath"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to placeable' task.
		/// </summary>
		/// <param name="targetEntity">The target placeable entity.</param>
		public TaskGoToPlaceable(ModelEntity targetEntity)
		:	base(new AlwaysRetry())
		{
			TargetEntityPath = targetEntity.GetAbsolutePath();
		}

		/// <summary>
		/// Constructs a 'go to placeable' task.
		/// </summary>
		/// <param name="targetEntityPath">The absolute path of the target placeable entity.</param>
		public TaskGoToPlaceable(string targetEntityPath)
		:	base(new AlwaysRetry())
		{
			TargetEntityPath = targetEntityPath;
		}

		/// <summary>
		/// Constructs a 'go to placeable' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToPlaceable(XElement element)
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
			ModelEntity targetEntity = entity.GetEntityByAbsolutePath(TargetEntityPath);

			var mobileComponent = entity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
			var placeableComponent = targetEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			var playingAreaComponent = entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

			// If the mobile entity's not currently within a playing area, return null.
			if(playingAreaComponent == null) return null;

			// Try and find a path to the nearest entrance of the target entity.
			List<Vector2> targetEntrances = placeableComponent.Entrances.Select(v => v.ToVector2()).ToList();
			Queue<Vector2> path = playingAreaComponent.NavigationMap.FindPath
			(
				mobileComponent.Position,
				targetEntrances,
				mobileComponent.Properties
			);

			// If a path has been found, return a task that will cause the entity to follow it, else return null.
			return path != null ? new TaskFollowPath(entity, path) : null;
		}

		#endregion
	}
}
