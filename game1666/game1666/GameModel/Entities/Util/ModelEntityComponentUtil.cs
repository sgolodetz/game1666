/***
 * game1666: ModelEntityComponentUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components.Context;

namespace game1666.GameModel.Entities.Util
{
	/// <summary>
	/// This class provides utility methods that make it easier to access the
	/// contents of the world's context component, e.g. the message system.
	/// </summary>
	static class ModelEntityComponentUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the destruction manager for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its destruction manager.</returns>
		public static IModelEntityDestructionManager DestructionManager(this ModelEntityComponent component)
		{
			return component.Context().DestructionManager;
		}

		/// <summary>
		/// Gets the message system for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its message system.</returns>
		public static MessageSystem MessageSystem(this ModelEntityComponent component)
		{
			return component.Context().MessageSystem;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the context component for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its context component.</returns>
		private static ContextComponent Context(this ModelEntityComponent component)
		{
			return component.Entity.GetRootEntity().GetComponent(ContextComponent.StaticGroup);
		}

		#endregion
	}
}
