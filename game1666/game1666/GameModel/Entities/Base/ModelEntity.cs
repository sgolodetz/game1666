/***
 * game1666: ModelEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Messaging;
using game1666.GameModel.Entities.Lifetime;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Base
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game model.
	/// </summary>
	sealed class ModelEntity : Entity<IModelEntity>, IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A manager that is used to ensure orderly destruction of entities.
		/// </summary>
		private EntityDestructionManager<IModelEntity> m_destructionManager;

		/// <summary>
		/// A message system that is used to dispatch messages across the game.
		/// </summary>
		private MessageSystem m_messageSystem = new MessageSystem();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A manager that is used to ensure orderly destruction of entities.
		/// </summary>
		public EntityDestructionManager<IModelEntity> DestructionManager { get { return m_destructionManager; } }

		/// <summary>
		/// A message system that is used to dispatch messages across the game.
		/// </summary>
		public MessageSystem MessageSystem { get { return m_messageSystem; } }

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override IModelEntity Self { get { return this; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity directly from its name and archetype.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		public ModelEntity(string name, string archetype)
		:	base(name, archetype)
		{
			ResetMessageSystemAndDestructionManager();
		}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public ModelEntity(XElement entityElt)
		:	base(entityElt)
		{
			ResetMessageSystemAndDestructionManager();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after this entity is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			m_messageSystem = Parent.MessageSystem;
			m_destructionManager = Parent.DestructionManager;

			m_messageSystem.RegisterRule
			(
				new MessageRule<EntityDestructionMessage>
				{
					Action = new Action<EntityDestructionMessage>(msg => Parent.RemoveChild(this)),
					Entities = new List<dynamic> { this, Parent },
					Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(this),
					Key = Guid.NewGuid().ToString()
				}
			);

			base.AfterAdd();
		}

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();
			ResetMessageSystemAndDestructionManager();
		}

		/// <summary>
		/// Tests whether or not this entity is equal to another one.
		/// </summary>
		/// <param name="rhs">The other entity.</param>
		/// <returns>true, if the two entities are equal, or false otherwise.</returns>
		public bool Equals(IModelEntity rhs)
		{
			return object.ReferenceEquals(this, rhs);
		}

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// If this is the root entity of the tree (i.e. the world), flush the entity destruction queue.
			if(Parent == null)
			{
				m_destructionManager.FlushQueue();
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Resets the message system and entity destruction manager. This is done both
		/// on initialisation and when removing an entity from its parent.
		/// </summary>
		private void ResetMessageSystemAndDestructionManager()
		{
			m_messageSystem = new MessageSystem();
			m_destructionManager = new EntityDestructionManager<IModelEntity>(m_messageSystem);
		}

		#endregion
	}
}
