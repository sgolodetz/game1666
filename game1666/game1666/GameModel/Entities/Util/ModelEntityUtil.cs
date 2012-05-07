/***
 * game1666: ModelEntityUtil.cs
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
	static class ModelEntityUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the entity destruction manager for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its entity destruction manager.</returns>
		public static IModelEntityDestructionManager EntityDestructionManager(this IModelEntity entity)
		{
			return entity.Context().EntityDestructionManager;
		}

		/// <summary>
		/// Gets the entity destruction manager for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its entity destruction manager.</returns>
		public static IModelEntityDestructionManager EntityDestructionManager(this ModelEntityComponent component)
		{
			return component.Context().EntityDestructionManager;
		}

		/// <summary>
		/// Gets the entity factory for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its entity factory.</returns>
		public static IModelEntityFactory EntityFactory(this IModelEntity entity)
		{
			return entity.Context().EntityFactory;
		}

		/// <summary>
		/// Gets the entity factory for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its entity factory.</returns>
		public static IModelEntityFactory EntityFactory(this ModelEntityComponent component)
		{
			return component.Context().EntityFactory;
		}

		/// <summary>
		/// Gets the message system for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its message system.</returns>
		public static MessageSystem MessageSystem(IModelEntity entity)
		{
			return entity.Context().MessageSystem;
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
		/// Gets the context component for the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its context component.</returns>
		private static ModelContextComponent Context(this IModelEntity entity)
		{
			return entity.GetRootEntity().GetComponent(ModelContextComponent.StaticGroup);
		}

		/// <summary>
		/// Gets the context component for the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its context component.</returns>
		private static ModelContextComponent Context(this ModelEntityComponent component)
		{
			return component.Entity.Context();
		}

		#endregion
	}
}
