/***
 * game1666: UIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Base
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game's UI.
	/// </summary>
	class UIEntity : Entity<IUIEntity>, IUIEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override IUIEntity Self { get { return this; } }

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get { return Properties["Viewport"]; } }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		public IModelEntity World { get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity with the specified viewport.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		/// <param name="viewport">The entity's viewport.</param>
		protected UIEntity(string name, string archetype, Viewport viewport)
		:	base(name, archetype)
		{
			Properties = new Dictionary<string,dynamic>
			{
				{ "Name", name },
				{ "Viewport", viewport }
			};
		}

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public UIEntity(XElement entityElt)
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
			World = Parent.World;
			base.AfterAdd();
		}

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();
			World = null;
		}

		#endregion
	}
}
