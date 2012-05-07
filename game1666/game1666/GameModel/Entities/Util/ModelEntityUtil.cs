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
	/// This class provides extension methods for entities and components in a
	/// model entity tree that allow them to easily access the contents of the
	/// context component stored in the root entity of their tree (i.e. the world).
	/// The context component stores the world's message system, entity factory
	/// and entity destruction queue, which are shared between all of the entities
	/// within a given world.
	/// </summary>
	static class ModelEntityUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the entity destruction manager for the world containing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its world's entity destruction manager.</returns>
		public static IModelEntityDestructionManager EntityDestructionManager(this IModelEntity entity)
		{
			return entity.Context().EntityDestructionManager;
		}

		/// <summary>
		/// Gets the entity destruction manager for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's entity destruction manager.</returns>
		public static IModelEntityDestructionManager EntityDestructionManager(this ModelEntityComponent component)
		{
			return component.Context().EntityDestructionManager;
		}

		/// <summary>
		/// Gets the entity factory for the world containing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its world's entity factory.</returns>
		public static IModelEntityFactory EntityFactory(this IModelEntity entity)
		{
			return entity.Context().EntityFactory;
		}

		/// <summary>
		/// Gets the entity factory for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's entity factory.</returns>
		public static IModelEntityFactory EntityFactory(this ModelEntityComponent component)
		{
			return component.Context().EntityFactory;
		}

		/// <summary>
		/// Gets the message system for the world containing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its world's message system.</returns>
		public static MessageSystem MessageSystem(IModelEntity entity)
		{
			return entity.Context().MessageSystem;
		}

		/// <summary>
		/// Gets the message system for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's message system.</returns>
		public static MessageSystem MessageSystem(this ModelEntityComponent component)
		{
			return component.Context().MessageSystem;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the context component for the world containing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its world's context component.</returns>
		private static ModelContextComponent Context(this IModelEntity entity)
		{
			return entity.GetRootEntity().GetComponent(ModelContextComponent.StaticGroup);
		}

		/// <summary>
		/// Gets the context component for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's context component.</returns>
		private static ModelContextComponent Context(this ModelEntityComponent component)
		{
			return component.Entity.Context();
		}

		#endregion
	}
}
