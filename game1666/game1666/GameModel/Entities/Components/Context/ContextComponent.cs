/***
 * game1666: ContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components.Context
{
	/// <summary>
	/// An instance of this component provides game context objects such as the message system
	/// and entity destruction manager to an entity tree. It is intended for use as a component
	/// of the root entity of the tree, i.e. the world. Other entities contained within the world
	/// can then get the world's message system or entity destruction manager by looking up this
	/// component via the tree.
	/// </summary>
	sealed class ContextComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that is used to ensure orderly destruction of entities.
		/// </summary>
		public IModelEntityDestructionManager DestructionManager { get; private set; }

		/// <summary>
		/// A factory that is used to construct model entities.
		/// </summary>
		public IModelEntityFactory Factory { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// A message system that is used to dispatch messages across the world.
		/// </summary>
		public MessageSystem MessageSystem { get; private set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Context"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameModel/Context"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a context component.
		/// </summary>
		public ContextComponent(IModelEntityFactory factory, IModelEntityDestructionManager destructionManager)
		{
			MessageSystem = new MessageSystem();

			Factory = factory;

			DestructionManager = destructionManager;
			DestructionManager.MessageSystem = MessageSystem;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			DestructionManager.FlushQueue();
		}

		#endregion
	}
}
