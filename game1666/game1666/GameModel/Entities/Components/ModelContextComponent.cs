/***
 * game1666: ModelContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components
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
		public override string Group { get { return ComponentGroups.CONTEXT; } }

		/// <summary>
		/// A message system that is used for indirect inter-entity communication within a world.
		/// </summary>
		public MessageSystem MessageSystem { get; private set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "ModelContext"; } }

		/// <summary>
		/// A factory that can be used to construct tasks.
		/// </summary>
		public ITaskFactory TaskFactory { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a model context component.
		/// </summary>
		/// <param name="entityFactory">A factory that can be used to construct model entities.</param>
		/// <param name="entityDestructionManager">A manager that can be used used to ensure orderly destruction of model entities.</param>
		/// <param name="taskFactory">A factory that can be used to construct tasks.</param>
		public ModelContextComponent(IModelEntityFactory entityFactory,
									 IModelEntityDestructionManager entityDestructionManager,
									 ITaskFactory taskFactory)
		{
			MessageSystem = new MessageSystem();

			EntityFactory = entityFactory;

			EntityDestructionManager = entityDestructionManager;
			EntityDestructionManager.MessageSystem = MessageSystem;

			TaskFactory = taskFactory;
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
