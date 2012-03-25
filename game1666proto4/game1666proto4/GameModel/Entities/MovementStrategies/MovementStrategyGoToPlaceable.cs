/***
 * game1666proto4: MovementStrategyGoToPlaceable.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Messages;
using game1666proto4.GameModel.Entities.Core;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities.MovementStrategies
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile
	/// entity to head towards the nearest entrance of the specified placeable entity.
	/// </summary>
	sealed class MovementStrategyGoToPlaceable : MovementStrategyGoToBase
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The placeable entity towards which to head.
		/// </summary>
		private IPlaceableEntity m_targetEntity;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to placeable' movement strategy from a target placeable entity.
		/// </summary>
		/// <param name="targetEntity">The target entity.</param>
		public MovementStrategyGoToPlaceable(IPlaceableEntity targetEntity)
		:	this(targetEntity.GetAbsolutePath())
		{}

		/// <summary>
		/// Constructs a 'go to placeable' movement strategy from the absolute path of a target placeable entity.
		/// </summary>
		/// <param name="targetPath">The absolute path of the target entity.</param>
		public MovementStrategyGoToPlaceable(string targetPath)
		{
			Properties = new Dictionary<string,dynamic>();
			Properties.Add("TargetPath", targetPath);
		}

		/// <summary>
		/// Constructs a 'go to placeable' movement strategy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the strategy's XML representation.</param>
		public MovementStrategyGoToPlaceable(XElement entityElt)
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
			// Look up the target entity (if it hasn't already been looked up).
			SetupTargetEntity();

			// Try and find a path to the nearest entrance of the target entity.
			Vector2 pos = EntityProperties["Position"];
			List<Vector2> targetEntrances = m_targetEntity.Entrances.Select(v => new Vector2(v.X + 0.5f, v.Y + 0.5f)).ToList();
			Queue<Vector2> path = NavigationMap.FindPath(pos, targetEntrances, EntityProperties);

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

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Looks up the target entity (if it hasn't already been looked up).
		/// </summary>
		private void SetupTargetEntity()
		{
			if(m_targetEntity != null) return;

			INamedEntity currentEntity = EntityProperties["Self"] as INamedEntity;
			m_targetEntity = currentEntity.GetEntityByAbsolutePath(Properties["TargetPath"] as string);
		}

		#endregion
	}
}
