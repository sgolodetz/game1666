/***
 * game1666: ModelEntityComponentExtensions.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Entities.Interfaces.Context;
using game1666.GameModel.Matchmaking;

namespace game1666.GameModel.Entities.Extensions
{
	/// <summary>
	/// This class provides extension methods for components in a model entity tree
	/// that allow easy access to the contents of the context component stored in
	/// the root entity of the tree (i.e. the world). The context component stores
	/// things like the world's message system and entity factory, which are shared
	/// between all of the entities within a given world.
	/// </summary>
	static class ModelEntityComponentExtensions
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

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
		/// Gets the resource matchmaker for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's resource matchmaker.</returns>
		public static ResourceMatchmaker Matchmaker(this ModelEntityComponent component)
		{
			return component.Context().Matchmaker;
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

		/// <summary>
		/// Gets the task factory for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's task factory.</returns>
		public static ITaskFactory TaskFactory(this ModelEntityComponent component)
		{
			return component.Context().TaskFactory;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the context component for the world containing the specified component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <returns>Its world's context component.</returns>
		private static IModelContextComponent Context(this ModelEntityComponent component)
		{
			return component.Entity.GetRootEntity().GetComponent<IModelContextComponent>(ModelEntityComponentGroups.CONTEXT);
		}

		#endregion
	}
}
