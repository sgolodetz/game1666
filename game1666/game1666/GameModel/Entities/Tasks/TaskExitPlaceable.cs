/***
 * game1666: TaskExitPlaceable.cs
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
	/// An instance of this class represents a task that causes a mobile entity to exit
	/// its containing placeable entity (provided that it is currently located at one of
	/// the placeable entity's entrances).
	/// </summary>
	sealed class TaskExitPlaceable : Task
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an 'exit placeable' task.
		/// </summary>
		public TaskExitPlaceable()
		{}

		/// <summary>
		/// Constructs an 'exit placeable' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskExitPlaceable(XElement element)
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
			// Provided that it is currently at one of the entrances (if any) of its containing
			// entity, the executing entity will leave its containing entity and appear on the
			// playing area of its containing entity's parent.
			ModelEntity executingEntity = entity;
			var mobileComponent = executingEntity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);

			ModelEntity containingEntity = executingEntity.Parent;
			int? entranceIndex = null;
			var playingAreaComponent = containingEntity.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			if(playingAreaComponent != null)
			{
				// If the containing entity has a playing area (e.g. it's a city), check whether the
				// entity is at one of the playing area entrances. If so, use that to determine the
				// place on the new playing area at which the entity should appear; if not, the entity
				// needs to head to one of the playing area entrances before it can leave.
				entranceIndex = MathUtil.FindIndexOfNearestNearbyPoint(playingAreaComponent.Entrances, mobileComponent.Position);
				if(entranceIndex == null) return TaskState.FAILED;
			}
			else
			{
				// If the containing entity has no playing area (e.g. it's a building), the entity
				// will leave it by an arbitrary entrance (currently always the first).
				entranceIndex = 0;
			}

			// If the mobile entity can leave, remove it from its containing entity and add it
			// to the parent of its containing entity, setting its position to the location of
			// the chosen (external) entrance.
			containingEntity.RemoveChild(executingEntity);
			var placeableComponent = containingEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			mobileComponent.Position = placeableComponent.Entrances[entranceIndex.Value].ToVector2();
			containingEntity.Parent.AddChild(executingEntity);
			return TaskState.SUCCEEDED;
		}

		#endregion
	}
}
