/***
 * game1666: ModelContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components.Context
{
	/// <summary>
	/// An instance of this component provides model context objects such as the message system and
	/// entity destruction manager to a model entity tree. It is intended for use as a component of
	/// the root entity of such a tree, i.e. the world. Other entities contained within the world
	/// can then access the world's message system or entity destruction manager by looking up this
	/// component via the tree.
	/// </summary>
	sealed class ModelContextComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that can be used used to ensure orderly destruction of model entities.
		/// </summary>
		public IModelEntityDestructionManager EntityDestructionManager { get; private set; }

		/// <summary>
		/// A factory that can be used to construct model entities.
		/// </summary>
		public IModelEntityFactory EntityFactory { get; private set; }

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
		public override string Name { get { return "ModelContext"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameModel/Context"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a model context component.
		/// </summary>
		/// <param name="entityFactory">A factory that can be used to construct model entities.</param>
		/// <param name="entityDestructionManager">A manager that can be used used to ensure orderly
		/// destruction of model entities.</param>
		public ModelContextComponent(IModelEntityFactory entityFactory, IModelEntityDestructionManager entityDestructionManager)
		{
			MessageSystem = new MessageSystem();

			EntityFactory = entityFactory;

			EntityDestructionManager = entityDestructionManager;
			EntityDestructionManager.MessageSystem = MessageSystem;
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
			EntityDestructionManager.FlushQueue();
		}

		#endregion
	}
}
