/***
 * game1666: TaskLeaveEntity.cs
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
	/// An instance of this class represents a task that causes a mobile entity to leave its
	/// containing entity. If the containing entity has a playing area, this task causes the
	/// mobile entity to head towards one of the entrances of the playing area before trying
	/// to exit the containing entity; if not, it just exits immediately.
	/// </summary>
	sealed class TaskLeaveEntity : RetryableTask
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'leave entity' task.
		/// </summary>
		public TaskLeaveEntity()
		:	base(new AlwaysRetry())
		{}

		/// <summary>
		/// Constructs a 'leave entity' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskLeaveEntity(XElement element)
		:	base(new AlwaysRetry(), element)
		{}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Generates a sequence sub-task that does the actual work.
		/// </summary>
		/// <param name="entity">The entity that will execute the sequence sub-task.</param>
		/// <returns>The generated sequence sub-task.</returns>
		protected override Task GenerateSubTask(dynamic entity)
		{
			var result = new SequenceTask();

			IPlayingAreaComponent playingAreaComponent = entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			if (playingAreaComponent != null)
			{
				result.AddTask(new TaskGoToALocalPosition(playingAreaComponent.Entrances.Select(v => v.ToVector2()).ToList(), new NeverRetry()));
			}

			result.AddTask(new TaskExitEntity());
			return result;
		}

		#endregion
	}
}
