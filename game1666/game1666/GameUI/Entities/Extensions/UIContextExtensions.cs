﻿/***
 * game1666: UIContextExtensions.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Interfaces.Components;

namespace game1666.GameUI.Entities.Extensions
{
	/// <summary>
	/// This class provides extension methods for entities and components in a UI entity
	/// tree that allow them to easily access the contents of the context component stored
	/// in the root entity of their tree (i.e. a game view). The context component contains
	/// a reference to the world being viewed.
	/// </summary>
	static class UIContextExtensions
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Looks up the target of the specified UI entity in the game model.
		/// </summary>
		/// <param name="entity">The UI entity whose target we wish to determine.</param>
		/// <returns>The UI entity's target, if any, or null otherwise.</returns>
		public static ModelEntity Target(this IUIEntity entity)
		{
			dynamic dynamicTargetPath = null;
			if(entity.Properties.TryGetValue("Target", out dynamicTargetPath))
			{
				string targetPath = dynamicTargetPath;
				if(targetPath == "./settlement:Home") targetPath = entity.World().Properties["HomeSettlement"];
				return entity.World().GetEntityByAbsolutePath(targetPath);
			}
			else return null;
		}

		/// <summary>
		/// Looks up a specified component of the target of the specified UI entity in the game model.
		/// </summary>
		/// <param name="entity">The UI entity a component of whose target we wish to get.</param>
		/// <param name="componentGroup">The group of the specified component.</param>
		/// <returns>The specified component of the UI entity's target, if any, or null otherwise.</returns>
		public static T TargetComponent<T>(this IUIEntity entity, string componentGroup) where T : class, IEntityComponent
		{
			ModelEntity targetEntity = entity.Target();
			if(targetEntity == null) return null;
			return targetEntity.GetComponent<T>(componentGroup);
		}

		/// <summary>
		/// Gets the world being viewed by the game view containing the specified UI entity.
		/// </summary>
		/// <param name="entity">The UI entity.</param>
		/// <returns>The world being viewed by the containing game view.</returns>
		public static ModelEntity World(this IUIEntity entity)
		{
			return entity.Context().World;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the context component for the game view containing the specified UI entity.
		/// </summary>
		/// <param name="entity">The UI entity.</param>
		/// <returns>The context component for the containing game view.</returns>
		private static IUIContextComponent Context(this IUIEntity entity)
		{
			return entity.GetRootEntity().GetComponent<IUIContextComponent>(UIEntityComponentGroups.CONTEXT);
		}

		/// <summary>
		/// Gets the context component for the game view containing the specified UI component.
		/// </summary>
		/// <param name="component">The UI component.</param>
		/// <returns>The context component for the containing game view.</returns>
		private static IUIContextComponent Context(this UIEntityComponent component)
		{
			return component.Entity.Context();
		}

		#endregion
	}
}
