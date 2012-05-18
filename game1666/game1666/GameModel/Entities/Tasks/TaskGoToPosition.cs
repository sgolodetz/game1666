/***
 * game1666: TaskGoToPosition.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Navigation;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile
	/// entity to head towards a specific position.
	/// </summary>
	sealed class TaskGoToPosition : RetryTask
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The properties of the mobile component of the entity that will be moving.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_mobileComponentProperties;

		/// <summary>
		/// The navigation map for the terrain on which the entity will be moving.
		/// </summary>
		private readonly ModelEntityNavigationMap m_navigationMap;

		/// <summary>
		/// The target position.
		/// </summary>
		private readonly Vector2 m_targetPosition;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' task from a target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		/// <param name="entityProperties">The properties of the mobile component of the entity that will be moving.</param>
		/// <param name="navigationMap">The navigation map for the terrain on which the entity will be moving.</param>
		public TaskGoToPosition(Vector2 targetPosition, IDictionary<string,dynamic> mobileComponentProperties, ModelEntityNavigationMap navigationMap)
		:	base(Int32.MaxValue)
		{
			m_targetPosition = targetPosition;
			m_mobileComponentProperties = mobileComponentProperties;
			m_navigationMap = navigationMap;
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
			// Try and find a path to the target position.
			Vector2 pos = m_mobileComponentProperties["Position"];
			Queue<Vector2> path = m_navigationMap.FindPath(pos, new List<Vector2> { m_targetPosition }, m_mobileComponentProperties);

			// If a path has been found, return a movement strategy that will cause the entity to follow it, else return null.
			if(path != null)
			{
				/*return new MovementStrategyFollowPath(path)
				{
					EntityProperties = this.EntityProperties,
					NavigationMap = this.NavigationMap
				};*/
				// TODO
				return null;
			}
			else return null;
		}

		#endregion
	}
}
