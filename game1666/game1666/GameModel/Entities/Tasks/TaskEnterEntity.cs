/***
 * game1666: TaskEnterEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity to enter
	/// a specific entity (provided that it is currently located at one of the entity's
	/// entrances).
	/// </summary>
	sealed class TaskEnterEntity : Task
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The absolute path of the target entity.
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
		/// Constructs an 'enter entity' task.
		/// </summary>
		/// <param name="targetEntity">The target entity.</param>
		public TaskEnterEntity(ModelEntity targetEntity)
		{
			TargetEntityPath = targetEntity.GetAbsolutePath();
		}

		/// <summary>
		/// Constructs an 'enter entity' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskEnterEntity(XElement element)
		:	base(element)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="entity">The entity that will execute the task.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(dynamic entity, GameTime gameTime)
		{
			ModelEntity executingEntity = entity;
			var mobileComponent = executingEntity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);

			ModelEntity targetEntity = executingEntity.GetEntityByAbsolutePath(TargetEntityPath);
			if(targetEntity == null) return TaskState.FAILED;
			var placeableComponent = targetEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);

			// If the mobile entity is at one of the target entity's entrances,
			// remove it from the playing area and add it to the target entity.
			int? entranceIndex = MathUtil.FindIndexOfNearestNearbyPoint(placeableComponent.Entrances, mobileComponent.Position);
			if(entranceIndex != null)
			{
				executingEntity.Parent.RemoveChild(executingEntity);

				// If the target entity has a playing area, map the mobile entity's
				// position to the playing area entrance that corresponds to the
				// entrance that was used to enter the target entity.
				var playingAreaComponent = targetEntity.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
				if(playingAreaComponent != null)
				{
					mobileComponent.Position = playingAreaComponent.Entrances[entranceIndex.Value].ToVector2();
				}

				targetEntity.AddChild(executingEntity);
				return TaskState.SUCCEEDED;
			}
			else return TaskState.FAILED;
		}

		#endregion
	}
}
