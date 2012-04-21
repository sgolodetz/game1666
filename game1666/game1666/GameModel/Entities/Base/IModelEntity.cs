/***
 * game1666: IModelEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.Entities;
using game1666.Common.Messaging;
using game1666.GameModel.Entities.Lifetime;

namespace game1666.GameModel.Entities.Base
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component-based entity that is part of the game model.
	/// </summary>
	interface IModelEntity : IEntity<IModelEntity>, IEquatable<IModelEntity>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that is used to ensure orderly destruction of entities.
		/// </summary>
		EntityDestructionManager<IModelEntity> DestructionManager { get; }

		/// <summary>
		/// A message system that is used to dispatch messages across the game.
		/// </summary>
		MessageSystem MessageSystem { get; }

		#endregion
	}
}
