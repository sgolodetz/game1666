/***
 * game1666: GameViewManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Persistence;
using game1666.Common.UI;
using game1666.GameUI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI
{
	/// <summary>
	/// An instance of this class manages the various different views for the game.
	/// </summary>
	sealed class GameViewManager : IPersistableObject
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
		private readonly IDictionary<string,UIEntity> m_views = new Dictionary<string,UIEntity>();

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
		public GameViewManager(XElement element, IEntity world)
		{
			ObjectPersister.LoadAndAddChildObjects
			(
				element,
				new ChildObjectAdder
				{
					CanBeUsedFor = t => t == typeof(UIEntity),
					AdditionalArguments = new object[] { world },
					AddAction = o => m_views[o.Name] = o
				}
			);

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
			// TODO
		}

		/// <summary>
		/// Saves the game view manager to XML.
		/// </summary>
		/// <returns>An XML representation of the game view manager.</returns>
		public XElement SaveToXML()
		{
			// TODO
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Updates the current view based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			// TODO
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
			// TODO
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		private void OnMousePressed(MouseState state)
		{
			// TODO
		}

		#endregion
	}
}
