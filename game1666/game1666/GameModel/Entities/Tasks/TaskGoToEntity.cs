/***
 * game1666: TaskGoToEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Tasks;
using game1666.Common.Tasks.RetryStrategies;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity to head
	/// towards a specific target entity within the world as a whole. Note that this
	/// is much more general than a 'go to local entity' task - it has to plan its
	/// way across the world in general, not just the local playing area.
	/// </summary>
	sealed class TaskGoToEntity : RetryableTask
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
		/// Constructs a 'go to entity' task.
		/// </summary>
		/// <param name="targetEntityPath">The absolute path of the target entity.</param>
		/// <param name="retryStrategy">The strategy determining the point at which the task should give up.</param>
		public TaskGoToEntity(string targetEntityPath, IRetryStrategy retryStrategy)
		:	base(new AlwaysRetry())
		{
			TargetEntityPath = targetEntityPath;
		}

		/// <summary>
		/// Constructs a 'go to entity' task from its XML representation.
		/// </summary>
		/// <param name="element">The root element of the task's XML representation.</param>
		public TaskGoToEntity(XElement element)
		:	base(new AlwaysRetry(), element)
		{}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Plans how to get to the target entity and generates a sequence sub-task that
		/// contains other tasks to do the actual work.
		/// </summary>
		/// <param name="entity">The entity that will execute the sub-task.</param>
		/// <returns>The generated sub-task.</returns>
		protected override Task GenerateSubTask(dynamic entity)
		{
			ModelEntity sourceEntity = entity.Parent;
			ModelEntity targetEntity = entity.GetEntityByAbsolutePath(TargetEntityPath);

			// Find the chains up, and then down, the entity tree that lead to the target entity.
			Tuple<List<ModelEntity>,List<ModelEntity>> chains = FindEntityChains(sourceEntity, targetEntity);

			// Generate the sequence sub-task based on these chains.
			var result = new SequenceTask();
			foreach(ModelEntity e in chains.Item1) result.AddTask(new TaskLeaveEntity());
			foreach(ModelEntity e in chains.Item2.Reverse<ModelEntity>()) result.AddTask(new TaskGoToLocalEntity(e));
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

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Finds the chains of entities we need to leave / enter to get to the target entity.
		/// To do this, we walk up the entity tree from both the source and target entities until
		/// reaching their common ancestor, accumulating the chains we need on the way.
		/// </summary>
		/// <param name="sourceEntity">The source entity (the parent of the entity executing the task).</param>
		/// <param name="targetEntity">The target entity (the entity towards which we want the executing entity to head).</param>
		/// <returns>A chain pair, of the form ([s1,...,sm], [dn,...,d1]). To get to the target entity, the executing
		/// entity must (in sequence) leave each entity si, and then navigate to and enter each entity di.</returns>
		private static Tuple<List<ModelEntity>,List<ModelEntity>> FindEntityChains(ModelEntity sourceEntity, ModelEntity targetEntity)
		{
			// Determine the levels of the source and target entities in the entity tree.
			// FIXME: There are much tidier ways of doing this.
			int sourceLevel = sourceEntity.GetAbsolutePath().Count(c => c == '/');
			int targetLevel = targetEntity.GetAbsolutePath().Count(c => c == '/');

			// Walk up the entity tree from both sides to accumulate the requisite chains.
			var chains = new Tuple<List<ModelEntity>,List<ModelEntity>>(new List<ModelEntity>(), new List<ModelEntity>());
			while(sourceEntity != targetEntity)
			{
				if(sourceLevel >= targetLevel)
				{
					chains.Item1.Add(sourceEntity);
					sourceEntity = sourceEntity.Parent;
					--sourceLevel;
				}
				else
				{
					chains.Item2.Add(targetEntity);
					targetEntity = targetEntity.Parent;
					--targetLevel;
				}
			}
			return chains;
		}

		#endregion
	}
}
