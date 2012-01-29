/***
 * game1666proto4: ToolUtil.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Entities;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// This class provides utility methods that can be used by the various tools.
	/// </summary>
	static class ToolUtil
	{
		/// <summary>
		/// Determine the 3D world space ray corresponding to the location of the user's mouse in the viewport.
		/// </summary>
		/// <param name="state">The current mouse state.</param>
		/// <param name="viewport">The viewport.</param>
		/// <param name="matProjection">The current projection matrix.</param>
		/// <param name="matView">The current view matrix.</param>
		/// <param name="matWorld">The current world matrix.</param>
		/// <returns>The ray.</returns>
		public static Ray DetermineMouseRay(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), matProjection, matView, matWorld);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), matProjection, matView, matWorld);

			// Find the ray (in world space) between them and return it.
			Vector3 dir = Vector3.Normalize(far - near);
			return new Ray(near, dir);
		}

		/// <summary>
		/// Tries to create an entity to be placed at the specified grid square.
		/// </summary>
		/// <param name="blueprintName">The name of the blueprint specifying the type of entity to create.</param>
		/// <param name="gridSquare">The terrain grid square on which to place the entity's hotspot.</param>
		/// <param name="orientation">The orientation of the entity.</param>
		/// <param name="terrain">The terrain on which the entity is to be placed.</param>
		/// <returns>The entity, if it can be created, or null otherwise.</returns>
		public static IPlaceableEntity TryCreateEntity(string blueprintName, Vector2i? gridSquare, Orientation4 orientation, Terrain terrain)
		{
			IPlaceableEntity entity = null;
			if(gridSquare != null)
			{
				// Work out what type of entity we're trying to place.
				Blueprint blueprint = BlueprintManager.GetBlueprint(blueprintName);
				Type entityType = blueprint.EntityType;

				// Attempt to determine the average altitude of the terrain beneath the entity's footprint.
				// Note that this will return null if the entity can't be validly placed.
				float? altitude = blueprint.Footprint.DetermineAverageAltitude(gridSquare.Value, terrain);

				// Provided the altitude could be determined, continue with entity creation.
				if(altitude != null)
				{
					// Set the properties of the entity.
					var entityProperties = new Dictionary<string,dynamic>();
					entityProperties["Altitude"] = altitude;
					entityProperties["Blueprint"] = blueprintName;
					entityProperties["Orientation"] = orientation;
					entityProperties["Position"] = gridSquare.Value;

					// Create the entity.
					entity = Activator.CreateInstance(entityType, entityProperties, EntityStateID.OPERATING) as IPlaceableEntity;
				}
			}
			return entity;
		}

		/// <summary>
		/// Tries to place an entity in a playing area, based on whether or not it is validly placed
		/// according to its own placement strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="playingArea">The playing area.</param>
		/// <returns>true, if the placement succeeded, or false otherwise</returns>
		public static bool TryPlaceEntity(IPlaceableEntity entity, IPlayingArea playingArea)
		{
			Contract.Requires(entity != null);
			Contract.Requires(playingArea != null);

			if(playingArea.OccupancyMap.IsValidlyPlaced(entity))
			{
				playingArea.AddDynamicEntity(entity.CloneNew());
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
