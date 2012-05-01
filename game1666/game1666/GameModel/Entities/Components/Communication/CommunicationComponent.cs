/***
 * game1666: CommunicationComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Messaging;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Lifetime;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Components.Communication
{
	/// <summary>
	/// An instance of this component provides inter-entity communication functionality
	/// to its containing entity. It is intended for use as a component of the world
	/// entity. Other entities contained within the world can then get the world's
	/// message system or entity destruction manager by looking up this component via
	/// the entity tree.
	/// </summary>
	sealed class CommunicationComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that is used to ensure orderly destruction of entities.
		/// </summary>
		public EntityDestructionManager<IModelEntity> DestructionManager { get; private set; }

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
		public override string Name { get { return "Communication"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameModel/Communication"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a communication component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public CommunicationComponent(XElement componentElt)
		:	base(componentElt)
		{
			MessageSystem = new MessageSystem();
			DestructionManager = new EntityDestructionManager<IModelEntity>(MessageSystem);
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
