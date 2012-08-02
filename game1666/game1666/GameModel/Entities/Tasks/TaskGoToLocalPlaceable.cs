/***
 * game1666: TaskGoToLocalPlaceable.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Tasks;
using game1666.Common.Tasks.RetryStrategies;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity to navigate
	/// to a specific placeable entity within its containing playing area and enter it.
	/// </summary>
	sealed class TaskGoToLocalPlaceable : RetryableTask
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
		/// Constructs a 'go to local placeable' task.
		/// </summary>
		/// <param name="targetEntity">The target placeable entity.</param>
		public TaskGoToLocalPlaceable(ModelEntity targetEntity)
		:	base(new AlwaysRetry())
		{
			TargetEntityPath = targetEntity.GetAbsolutePath();
		}

		/// <summary>
		/// Constructs a 'go to local placeable' task.
		/// </summary>
		/// <param name="targetEntityPath">The absolute path of the target placeable entity.</param>
		public TaskGoToLocalPlaceable(string targetEntityPath)
		:	base(new AlwaysRetry())
		{
			TargetEntityPath = targetEntityPath;
		}

		/// <summary>
		/// Constructs a 'go to local placeable' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToLocalPlaceable(XElement element)
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
			ModelEntity targetEntity = entity.GetEntityByAbsolutePath(TargetEntityPath);
			if(targetEntity == null) return null;

			// Try and navigate to the nearest entrance of the target entity and then enter it.
			var result = new SequenceTask();
			var placeableComponent = targetEntity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			result.AddTask(new TaskGoToALocalPosition(placeableComponent.Entrances.Select(v => v.ToVector2()).ToList(), new NeverRetry()));
			result.AddTask(new TaskEnterPlaceable(targetEntity));
			return result;
		}

		/// <summary>
		/// Makes the task fail irretrievably if the target entity no longer exists.
		/// </summary>
		/// <param name="entity">The entity that would execute a sub-task, were it still possible.</param>
		/// <returns>true, if the task can no longer succeed, or false otherwise.</returns>
		protected override bool NoLongerPossible(dynamic entity)
		{
			return entity.GetEntityByAbsolutePath(TargetEntityPath) == null;
		}

		#endregion
	}
}
