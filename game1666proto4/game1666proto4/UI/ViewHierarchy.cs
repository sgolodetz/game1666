/***
 * game1666proto4: ViewHierarchy.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class manages the view hierarchy for the game.
	/// </summary>
	sealed class ViewHierarchy : CompositeVisibleEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The current game view.
		/// </summary>
		private string m_currentView = "City";

		/// <summary>
		/// The different game views, e.g. City, World, etc.
		/// </summary>
		private readonly IDictionary<string,GameView> m_views = new Dictionary<string,GameView>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		public override IEnumerable<dynamic> Children { get { return m_views.Values; } }

		/// <summary>
		/// The current game view.
		/// </summary>
		public string CurrentView { get { return m_currentView; } set { m_currentView = value; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view hierarchy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the hierarchy's XML representation.</param>
		public ViewHierarchy(XElement entityElt)
		:	base(entityElt)
		{
			// Register input handlers.
			MouseEventManager.OnMouseMoved += OnMouseMoved;
			MouseEventManager.OnMousePressed += OnMousePressed;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the view hierarchy based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a view to the view hierarchy.
		/// </summary>
		/// <param name="view">The view.</param>
		public void AddEntity(GameView view)
		{
			m_views[view.Name] = view;
		}

		/// <summary>
		/// Draws the current view.
		/// </summary>
		public override void Draw()
		{
			m_views[m_currentView].Draw();
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			m_views[m_currentView].OnMouseMoved(state);
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			m_views[m_currentView].OnMousePressed(state);
		}

		/// <summary>
		/// Updates the current view based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			m_views[m_currentView].Update(gameTime);
		}

		#endregion
	}
}
