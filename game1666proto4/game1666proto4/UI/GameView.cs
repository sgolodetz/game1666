﻿/***
 * game1666proto4: GameView.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class represents a particular game view, e.g. the City View.
	/// </summary>
	sealed class GameView : CompositeVisibleEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The visible entities that together make up the view.
		/// </summary>
		private readonly IDictionary<string,IVisibleEntity> m_children = new Dictionary<string,IVisibleEntity>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		protected override IEnumerable<IVisibleEntity> Children { get { return m_children.Values; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the view's XML representation.</param>
		public GameView(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a visible entity (such as a playing area viewer) to the view.
		/// </summary>
		/// <param name="entity">The visible entity.</param>
		public void AddEntity(IVisibleEntity entity)
		{
			m_children[entity.Name] = entity;
		}

		/// <summary>
		/// Adds an entity to the view based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
