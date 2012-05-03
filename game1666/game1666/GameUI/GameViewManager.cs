/***
 * game1666: GameViewManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.Common.UI;
using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI
{
	/// <summary>
	/// An instance of this class manages the various different views for the game.
	/// </summary>
	sealed class GameViewManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The current game view.
		/// </summary>
		private string m_currentView = "gameview:City";

		/// <summary>
		/// The different game views, e.g. City, World, etc.
		/// </summary>
		private readonly IDictionary<string,IUIEntity> m_views = new Dictionary<string,IUIEntity>();

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
		/// Constructs a game view manager and loads its game views from their XML representation.
		/// </summary>
		/// <param name="element">The XML element representing the game view manager.</param>
		/// <param name="world">The world that is being viewed.</param>
		public GameViewManager(XElement element, IModelEntity world)
		{
			foreach(var t in ObjectPersister.LoadChildObjectsAndXML<IUIEntity>(element))
			{
				IUIEntity view = t.Item1;
				XElement viewElt = t.Item2;
				view.World = world;
				view.AddDescendantsFromXML(viewElt);
				m_views[view.Name] = view;
			}

			// Register input handlers.
			MouseEventManager.OnMouseMoved += OnMouseMoved;
			MouseEventManager.OnMousePressed += OnMousePressed;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the current view.
		/// </summary>
		public void Draw()
		{
			var renderer = m_views[m_currentView].GetComponent(ControlRenderingComponent.StaticGroup);
			if(renderer != null)
			{
				renderer.Draw();
			}
		}

		/// <summary>
		/// Updates the current view based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			m_views[m_currentView].Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		private void OnMouseMoved(MouseState state)
		{
			var interactor = m_views[m_currentView].GetComponent(InteractionComponent.StaticGroup);
			if(interactor != null)
			{
				interactor.OnMouseMoved(state);
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			var interactor = m_views[m_currentView].GetComponent(InteractionComponent.StaticGroup);
			if(interactor != null)
			{
				interactor.OnMousePressed(state);
			}
		}

		#endregion
	}
}
