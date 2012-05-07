/***
 * game1666: UIEntityUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Context;

namespace game1666.GameUI.Entities.Util
{
	/// <summary>
	/// This class provides utility methods that make it easier to look up the target of a UI entity,
	/// or a component of that target, in the game model.
	/// </summary>
	static class UIEntityUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the entity factory for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its entity factory.</returns>
		public static IUIEntityFactory EntityFactory(this UIEntityComponent component)
		{
			return component.Context().EntityFactory;
		}

		/// <summary>
		/// Looks up the target of the specified UI entity in the game model.
		/// </summary>
		/// <param name="entity">The UI entity whose target we wish to determine.</param>
		/// <returns>The UI entity's target, if any, or null otherwise.</returns>
		public static IModelEntity Target(this IUIEntity entity)
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
		public static dynamic TargetComponent(this IUIEntity entity, string componentGroup)
		{
			IModelEntity targetEntity = entity.Target();
			if(targetEntity == null) return null;
			return targetEntity.GetComponent(componentGroup);
		}

		/// <summary>
		/// Gets the world for the specified entity.
		/// </summary>
		/// <param name="component">The entity.</param>
		/// <returns>Its world.</returns>
		public static IModelEntity World(this IUIEntity entity)
		{
			return entity.Context().World;
		}

		/// <summary>
		/// Gets the world for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world.</returns>
		public static IModelEntity World(this UIEntityComponent component)
		{
			return component.Context().World;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the UI context component for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its context component.</returns>
		private static UIContextComponent Context(this IUIEntity entity)
		{
			return entity.GetRootEntity().GetComponent(UIContextComponent.StaticGroup);
		}

		/// <summary>
		/// Gets the UI context component for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its context component.</returns>
		private static UIContextComponent Context(this UIEntityComponent component)
		{
			return component.Entity.Context();
		}

		#endregion
	}
}
