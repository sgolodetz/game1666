/***
 * game1666: UIContextExtensions.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// This class provides extension methods for entities and components in a
	/// UI entity tree that allow them to easily access the contents of the
	/// context component stored in the root entity of their tree (i.e. a game
	/// view). The context component contains a reference to the world being
	/// viewed, and stores an entity factory that can be used to create new
	/// UI entities on the fly.
	/// </summary>
	static class UIContextExtensions
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the entity factory for the game view containing the specified UI component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>The entity factory for the containing game view.</returns>
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
		/// Gets the world being viewed by the game view containing the specified UI entity.
		/// </summary>
		/// <param name="entity">The UI entity.</param>
		/// <returns>The world being viewed by the containing game view.</returns>
		public static IModelEntity World(this IUIEntity entity)
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
		private static UIContextComponent Context(this IUIEntity entity)
		{
			return entity.GetRootEntity().GetComponent(UIContextComponent.StaticGroup);
		}

		/// <summary>
		/// Gets the context component for the game view containing the specified UI component.
		/// </summary>
		/// <param name="component">The UI component.</param>
		/// <returns>The context component for the containing game view.</returns>
		private static UIContextComponent Context(this UIEntityComponent component)
		{
			return component.Entity.Context();
		}

		#endregion
	}
}
