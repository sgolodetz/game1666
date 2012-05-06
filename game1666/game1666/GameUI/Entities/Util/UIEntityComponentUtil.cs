/***
 * game1666: UIEntityComponentUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Util
{
	/// <summary>
	/// This class provides utility methods that make it easier to look up the target of a UI entity,
	/// or a component of that target, in the game model.
	/// </summary>
	static class UIEntityComponentUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Looks up the target of the specified UI entity in the game model.
		/// </summary>
		/// <param name="entity">The UI entity whose target we wish to determine.</param>
		/// <returns>The UI entity's target, if any, or null otherwise.</returns>
		public static IModelEntity GetTarget(this IUIEntity entity)
		{
			dynamic dynamicTargetPath = null;
			if(entity.Properties.TryGetValue("Target", out dynamicTargetPath))
			{
				string targetPath = dynamicTargetPath;
				if(targetPath == "./settlement:Home") targetPath = entity.World.Properties["HomeSettlement"];
				return entity.World.GetEntityByAbsolutePath(targetPath);
			}
			else return null;
		}

		/// <summary>
		/// Looks up a specified component of the target of the specified UI entity in the game model.
		/// </summary>
		/// <param name="entity">The UI entity a component of whose target we wish to get.</param>
		/// <param name="componentGroup">The group of the specified component.</param>
		/// <returns>The specified component of the UI entity's target, if any, or null otherwise.</returns>
		public static dynamic GetTargetComponent(this IUIEntity entity, string componentGroup)
		{
			IModelEntity targetEntity = entity.GetTarget();
			if(targetEntity == null) return null;
			return targetEntity.GetComponent(componentGroup);
		}

		#endregion
	}
}
