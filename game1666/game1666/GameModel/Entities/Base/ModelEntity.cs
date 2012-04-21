/***
 * game1666: ModelEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Messaging;

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
		/// A message system that is used to dispatch messages across the game.
		/// </summary>
		private MessageSystem m_messageSystem = new MessageSystem();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A message system that is used to dispatch messages across the game.
		/// </summary>
		public MessageSystem MessageSystem
		{
			get { return m_messageSystem; }
		}

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
		{}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public ModelEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after this entity is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			m_messageSystem = Parent.MessageSystem;
			base.AfterAdd();
		}

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();
			m_messageSystem = new MessageSystem();
		}

		#endregion
	}
}
