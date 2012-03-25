/***
 * game1666proto4: MovementStrategyGoToPosition.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.MovementStrategies
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile
	/// entity to head towards a specific position.
	/// </summary>
	sealed class MovementStrategyGoToPosition : MovementStrategyGoToBase
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' movement strategy from a target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		public MovementStrategyGoToPosition(Vector2 targetPosition)
		{
			Properties = new Dictionary<string,dynamic>();
			Properties.Add("TargetPosition", targetPosition);
		}

		/// <summary>
		/// Constructs a 'go to position' movement strategy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the strategy's XML representation.</param>
		public MovementStrategyGoToPosition(XElement entityElt)
		{
			Properties = EntityPersister.LoadProperties(entityElt);
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Tries to generate an appropriate 'follow path' sub-strategy for the mobile entity to follow.
		/// </summary>
		/// <returns>The generated sub-strategy, if any, or null otherwise.</returns>
		protected override MovementStrategyFollowPath GenerateSubStrategy()
		{
			// Try and find a path to the target position.
			Vector2 pos = EntityProperties["Position"];
			Queue<Vector2> path = NavigationMap.FindPath(pos, new List<Vector2> { Properties["TargetPosition"] }, EntityProperties);

			// If a path has been found, return a movement strategy that will cause the entity to follow it, else return null.
			if(path != null)
			{
				return new MovementStrategyFollowPath(path)
				{
					EntityProperties = this.EntityProperties,
					NavigationMap = this.NavigationMap
				};
			}
			else return null;
		}

		#endregion
	}
}
