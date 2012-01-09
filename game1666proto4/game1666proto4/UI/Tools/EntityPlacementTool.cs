/***
 * game1666proto4: EntityPlacementTool.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// An instance of this class can be used to place entities in a playing area.
	/// </summary>
	sealed class EntityPlacementTool : ITool
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The orientation of the entity currently being placed (if any).
		/// </summary>
		private Orientation4 m_placementOrientation = Orientation4.XPOS;

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
		/// The name of the blueprint specifying the kind of entity to place.
		/// </summary>
		public string Name { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity placement tool.
		/// </summary>
		/// <param name="name">The name of the blueprint specifying the kind of entity to place.</param>
		/// <param name="playingArea">The playing area in which to place the entity.</param>
		public EntityPlacementTool(string name, IPlayingArea playingArea)
		{
			Name = name;
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
			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), matProjection, matView, matWorld);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), matProjection, matView, matWorld);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// Determine which grid square we're hovering over (if any).
			Vector2i? gridSquare = m_playingArea.Terrain.PickGridSquare(ray);

			Entity = null;
			if(gridSquare != null && (Name == "Dwelling" || Name == "Mansion" || Name == "Village"))
			{
				// Work out what type of entity we're trying to place.
				Blueprint blueprint = BlueprintManager.GetBlueprint(Name);
				Type entityType = blueprint.EntityType;

				// Attempt to determine the average altitude of the terrain beneath the entity's footprint.
				// Note that this will return null if the entity can't be validly placed.
				Footprint footprint = blueprint.Footprint.Rotated((int)m_placementOrientation);
				float? altitude = footprint.DetermineAverageAltitude(gridSquare.Value, m_playingArea.Terrain);

				// Provided the altitude could be determined, continue with entity creation.
				if(altitude != null)
				{
					// Set the properties of the entity.
					var entityProperties = new Dictionary<string,dynamic>();
					entityProperties["Altitude"] = altitude;
					entityProperties["Blueprint"] = Name;
					entityProperties["Orientation"] = m_placementOrientation;
					entityProperties["Position"] = gridSquare.Value;

					// Create the new entity, and set it as the entity to be placed if it's valid.
					Entity = Activator.CreateInstance(entityType, entityProperties, EntityStateID.OPERATING) as IPlaceableEntity;
				}
			}
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
			if(state.LeftButton == ButtonState.Pressed)
			{
				if(Entity != null && Entity.PlacementStrategy.IsValidlyPlaced(
					m_playingArea.Terrain,
					Entity.Blueprint.Footprint,
					Entity.Position,
					Entity.Orientation
				))
				{
					m_playingArea.AddPlaceableEntity(Entity.CloneNew());
					return null;
				}
			}
			else if(state.RightButton == ButtonState.Pressed)
			{
				int placementOrientation = (int)m_placementOrientation;
				placementOrientation = (placementOrientation + 1) % 4;
				m_placementOrientation = (Orientation4)placementOrientation;

				// Call the mouse moved handler to update the entity being placed.
				OnMouseMoved(state, viewport, matProjection, matView, matWorld);
			}
			return this;
		}

		#endregion
	}
}
