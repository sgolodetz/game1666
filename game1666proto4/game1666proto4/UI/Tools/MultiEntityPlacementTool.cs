﻿/***
 * game1666proto4: MultiEntityPlacementTool.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// An instance of this class can be used to place multiple entities in a playing area with a single drag of the mouse.
	/// </summary>
	sealed class MultiEntityPlacementTool : ITool
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The name of the blueprint specifying the kind of entity to place.
		/// </summary>
		private readonly string m_name;

		/// <summary>
		/// The playing area in which to place the entity.
		/// </summary>
		private readonly IPlayingArea m_playingArea;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity currently being placed by the user (if any).
		/// </summary>
		public IPlaceableEntity Entity { get; private set; }

		/// <summary>
		/// The name of the tool.
		/// </summary>
		public string Name { get { return "Place:" + m_name; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new multi-entity placement tool.
		/// </summary>
		/// <param name="name">The name of the blueprint specifying the kind of entity to place.</param>
		/// <param name="playingArea">The playing area in which to place the entities.</param>
		public MultiEntityPlacementTool(string name, IPlayingArea playingArea)
		{
			m_name = name;
			m_playingArea = playingArea;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		/// <param name="viewport">The viewport of the viewer being used to interact with the playing area.</param>
		/// <param name="matProjection">The current projection matrix in that viewer.</param>
		/// <param name="matView">The current view matrix in that viewer.</param>
		/// <param name="matWorld">The current world matrix in that viewer.</param>
		public void OnMouseMoved(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			// Determine the 3D world space ray corresponding to the location of the user's mouse in the viewport.
			var ray = ToolUtil.DetermineMouseRay(state, viewport, matProjection, matView, matWorld);

			// Determine which grid square we're hovering over (if any).
			Tuple<Vector2i,float> gridSquareAndDistance = m_playingArea.Terrain.PickGridSquare(ray);
			Vector2i? gridSquare = gridSquareAndDistance != null ? gridSquareAndDistance.Item1 : (Vector2i?)null;

			// Try to create an entity to be placed at the specified grid square.
			Entity = ToolUtil.TryCreateEntity(m_name, gridSquare, Orientation4.XPOS, m_playingArea.Terrain);

			// If the left mouse button is pressed and there is an entity to place, try and place it in the playing area.
			TryPlaceEntity(state);
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		/// <param name="viewport">The viewport of the viewer being used to interact with the playing area.</param>
		/// <param name="matProjection">The current projection matrix in that viewer.</param>
		/// <param name="matView">The current view matrix in that viewer.</param>
		/// <param name="matWorld">The current world matrix in that viewer.</param>
		/// <returns>The tool that should be active after the mouse press (generally this or null).</returns>
		public ITool OnMousePressed(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			// If the left mouse button is pressed and there is an entity to place, try and place it in the playing area.
			TryPlaceEntity(state);

			return this;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// If the left mouse button is pressed and there is an entity to place, try and place it in the playing area.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check for the calling event was made.</param>
		private void TryPlaceEntity(MouseState state)
		{
			if(state.LeftButton == ButtonState.Pressed && Entity != null)
			{
				ToolUtil.TryPlaceEntity(Entity, m_playingArea);
			}
		}

		#endregion
	}
}
