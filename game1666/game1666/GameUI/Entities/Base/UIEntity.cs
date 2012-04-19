/***
 * game1666: UIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Base
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game's UI.
	/// </summary>
	sealed class UIEntity : Entity<IUIEntity>, IUIEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		private IModelEntity m_world;

		#endregion

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
		public IModelEntity World
		{
			get	{ return ((UIEntity)GetRootEntity()).m_world; }
			set	{ ((UIEntity)GetRootEntity()).m_world = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public UIEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
