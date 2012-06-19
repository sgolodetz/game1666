/***
 * game1666: TaskEnterPlaceable.cs
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
	/// a specific placeable entity (provided that it is currently located at one of the
	/// placeable entity's entrances).
	/// </summary>
	sealed class TaskEnterPlaceable : Task
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
		/// Constructs an 'enter placeable' task.
		/// </summary>
		/// <param name="targetEntityPath">The absolute path of the target placeable entity.</param>
		public TaskEnterPlaceable(string targetEntityPath)
		{
			TargetEntityPath = targetEntityPath;
		}

		/// <summary>
		/// Constructs an 'enter placeable' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskEnterPlaceable(XElement element)
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
			var placeableComponent = targetEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);

			// If the mobile entity is at one of the target entity's entrances,
			// remove it from the playing area and add it to the target entity.
			for(int i = 0, count = placeableComponent.Entrances.Count; i < count; ++i)
			{
				Vector2i placeableEntranceSquare = placeableComponent.Entrances[i];
				var placeableEntrancePos = new Vector2(placeableEntranceSquare.X + 0.5f, placeableEntranceSquare.Y + 0.5f);
				if(Vector2.DistanceSquared(mobileComponent.Position, placeableEntrancePos) < Constants.EPSILON_SQUARED)
				{
					executingEntity.Parent.RemoveChild(executingEntity);

					// If the target entity has a playing area, map the mobile entity's
					// position to the playing area entrance that corresponds to the
					// entrance that was used to enter the target entity.
					var playingAreaComponent = targetEntity.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
					if(playingAreaComponent != null)
					{
						Vector2i playingAreaEntranceSquare = playingAreaComponent.Entrances[i];
						mobileComponent.Position = new Vector2(playingAreaEntranceSquare.X + 0.5f, playingAreaEntranceSquare.Y + 0.5f);
					}

					targetEntity.AddChild(executingEntity);

					return TaskState.SUCCEEDED;
				}
			}

			return TaskState.FAILED;
		}

		#endregion
	}
}
