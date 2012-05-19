﻿/***
 * game1666: IModelEntityExtensions.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components;

namespace game1666.GameModel.Entities.Extensions
{
	/// <summary>
	/// This class provides extension methods for entities in a model entity tree
	/// that allow easy access to the contents of the context component stored in
	/// the root entity of the tree (i.e. the world). The context component stores
	/// the world's message system, entity factory and entity destruction queue,
	/// which are shared between all of the entities within a given world.
	/// </summary>
	static class IModelEntityExtensions
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the entity factory for the world containing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Its world's entity factory.</returns>
		public static IModelEntityFactory EntityFactory(this IModelEntity entity)
		{
			return entity.Context().EntityFactory;
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

		#endregion
	}
}