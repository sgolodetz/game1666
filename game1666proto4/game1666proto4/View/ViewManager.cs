/***
 * game1666proto4: ViewManager.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class manages the view hierarchy for the game.
	/// </summary>
	sealed class ViewManager : CompositeViewEntity
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
		private IDictionary<string,View> m_views = new Dictionary<string,View>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The current game view.
		/// </summary>
		public string CurrentView { get { return m_currentView; } set { m_currentView = value; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view manager from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the manager's XML representation.</param>
		public ViewManager(XElement entityElt)
		:	base(entityElt)
		{
			// Register input handlers.
			MouseEventManager.OnMousePressed += OnMousePressed;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a view to the view manager.
		/// </summary>
		/// <param name="view">The view.</param>
		public void AddEntity(View view)
		{
			m_views[view.Name] = view;
		}

		/// <summary>
		/// Adds an entity to the view manager based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Draws the current view.
		/// </summary>
		public override void Draw()
		{
			m_views[m_currentView].Draw();
		}

		/// <summary>
		/// Gets an entity in the view manager by its (relative) path, e.g. "City".
		/// </summary>
		/// <param name="path">The path to the entity.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count != 0) return null;
			else return this;
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
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
