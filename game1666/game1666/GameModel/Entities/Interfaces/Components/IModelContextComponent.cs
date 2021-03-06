﻿/***
 * game1666: IModelContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.Messaging;
using game1666.GameModel.Entities.Interfaces.Context;
using game1666.GameModel.Matchmaking;

namespace game1666.GameModel.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides model context objects such as the
	/// message system and entity destruction manager to a model entity tree. It is intended for use
	/// as a component of the root entity of such a tree, i.e. the world. Other entities contained
	/// within the world can then access the world's message system or entity destruction manager by
	/// looking up this component via the tree.
	/// </summary>
	interface IModelContextComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that can be used used to ensure orderly destruction of model entities.
		/// </summary>
		IModelEntityDestructionManager EntityDestructionManager { get; }

		/// <summary>
		/// A matchmaker that is used to match up requests and offers of game resources.
		/// </summary>
		ResourceMatchmaker Matchmaker { get; }

		/// <summary>
		/// A message system that is used for indirect inter-entity communication within a world.
		/// </summary>
		MessageSystem MessageSystem { get; }

		/// <summary>
		/// A factory that can be used to construct tasks.
		/// </summary>
		ITaskFactory TaskFactory { get; }

		#endregion		
	}
}
