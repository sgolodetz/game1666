/***
 * game1666proto4: GameView.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

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

		/// <summary>
		/// The state shared between the visible entities that together make up the game view - e.g. things like the currently active tool.
		/// </summary>
		private readonly GameViewState m_state = new GameViewState();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		public override IEnumerable<dynamic> Children { get { return m_children.Values; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the view's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public GameView(XElement entityElt, INamedEntity world)
		:	base(entityElt, world)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the view based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			entity.GameViewState = m_state;
			AddEntity(entity);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Adds a visible entity (such as a playing area viewer) to the view.
		/// </summary>
		/// <param name="entity">The visible entity.</param>
		private void AddEntity(IVisibleEntity entity)
		{
			m_children[entity.Name] = entity;
		}

		#endregion
	}
}
