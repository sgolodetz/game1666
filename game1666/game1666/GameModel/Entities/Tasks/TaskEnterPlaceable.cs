/***
 * game1666: TaskEnterPlaceable.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Maths;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components;
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
			// Determine the position of the mobile entity.
			ModelEntity executingEntity = entity;
			Vector2 pos = executingEntity.GetComponent<MobileComponent>(ModelEntityComponentGroups.EXTERNAL).Position;

			// If the mobile entity is at one of the target entity's entrances,
			// remove it from the playing area and add it to the target entity.
			ModelEntity targetEntity = executingEntity.GetEntityByAbsolutePath(TargetEntityPath);
			var placeableComponent = targetEntity.GetComponent<PlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			foreach(Vector2i entranceSquare in placeableComponent.Entrances)
			{
				var entrancePos = new Vector2(entranceSquare.X + 0.5f, entranceSquare.Y + 0.5f);
				if(Vector2.DistanceSquared(pos, entrancePos) < Constants.EPSILON_SQUARED)
				{
					executingEntity.Parent.RemoveChild(executingEntity);
					targetEntity.AddChild(executingEntity);
					return TaskState.SUCCEEDED;
				}
			}

			return TaskState.FAILED;
		}

		#endregion
	}
}
