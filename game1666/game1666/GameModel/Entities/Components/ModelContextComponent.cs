/***
 * game1666: ModelContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Entities.Interfaces.Context;
using game1666.GameModel.Matchmaking;
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
	sealed class ModelContextComponent : ModelEntityComponent, IModelContextComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that can be used used to ensure orderly destruction of model entities.
		/// </summary>
		public IModelEntityDestructionManager EntityDestructionManager { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.CONTEXT; } }

		/// <summary>
		/// A matchmaker that is used to match up requests and offers of game resources.
		/// </summary>
		public ResourceMatchmaker Matchmaker { get; private set; }

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
		/// <param name="entityDestructionManager">A manager that can be used used to ensure orderly destruction of model entities.</param>
		/// <param name="taskFactory">A factory that can be used to construct tasks.</param>
		public ModelContextComponent(IModelEntityDestructionManager entityDestructionManager, ITaskFactory taskFactory)
		{
			Matchmaker = new ResourceMatchmaker();
			MessageSystem = new MessageSystem();

			EntityDestructionManager = entityDestructionManager;
			EntityDestructionManager.MessageSystem = MessageSystem;

			TaskFactory = taskFactory;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the component to XML.
		/// </summary>
		/// <returns>An XML representation of the component.</returns>
		public override XElement SaveToXML()
		{
			// The model context component is not persisted between runs
			// of the game (it is recreated when the world is reloaded).
			return null;
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			Matchmaker.Match();
			EntityDestructionManager.FlushQueue();
		}

		#endregion
	}
}
