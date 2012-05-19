﻿/***
 * game1666: ToolUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using game1666.Common.Maths;
using game1666.GameModel.Entities.AbstractComponents;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Components;
using game1666.GameModel.Entities.Extensions;
using game1666.GameModel.Terrains;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Tools
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
		/// <param name="factory">The factory to be used to create the entity.</param>
		/// <param name="percentComplete">The percentage of the entity that has been constructed.</param>
		/// <returns>The entity, if it can be created, or null otherwise.</returns>
		public static IModelEntity TryCreateEntity(string blueprintName, Vector2i? gridSquare, Orientation4 orientation, Terrain terrain, IModelEntityFactory factory, int percentComplete)
		{
			// If there's no grid square on which to place the entity's hotspot, early out.
			if(gridSquare == null) return null;

			// Work out what type of entity we're trying to place.
			PlaceableBlueprint blueprint = BlueprintManager.GetBlueprint(blueprintName);
			string archetype = blueprint.Archetype;

			// Attempt to determine the average altitude of the terrain beneath the entity's footprint.
			// Note that this will return null if the entity can't be validly placed.
			float? altitude = blueprint.Footprint.Rotated((int)orientation).DetermineAverageAltitude(gridSquare.Value, terrain);
			if(altitude == null) return null;

			// Provided the altitude could be determined, continue with entity creation.
			var properties = new Dictionary<string,dynamic>();
			properties["Altitude"] = altitude;
			properties["Blueprint"] = blueprintName;
			properties["ConstructionDone"] = blueprint.TimeToConstruct * percentComplete / 100;
			properties["Orientation"] = orientation;
			properties["Position"] = gridSquare.Value;
			properties["State"] = percentComplete < 100 ? "IN_CONSTRUCTION" : "OPERATING";
			return factory.MakeEntity(archetype, properties);
		}

		/// <summary>
		/// Tries to place an entity on the terrain of an entity that has a playing area,
		/// succeeding if and only if it is validly placed.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="playingAreaEntity">The entity that has the playing area.</param>
		/// <returns>true, if the placement succeeded, or false otherwise.</returns>
		public static bool TryPlaceEntity(IModelEntity entity, IModelEntity playingAreaEntity)
		{
			Contract.Requires(entity != null);
			Contract.Requires(playingAreaEntity != null);

			IPlaceableComponent placeableComponent = entity.GetComponent(ModelEntityComponentGroups.PLACEABLE);

			if(placeableComponent.IsValidlyPlaced(playingAreaEntity))
			{
				IPlayingAreaComponent playingAreaComponent = playingAreaEntity.GetComponent(ModelEntityComponentGroups.PLAYING_AREA);

				// Note: We know that the call to TryCreateEntity will always succeed, since the entity
				// that has been passed in was successfully created with almost identical properties.
				playingAreaEntity.AddChild
				(
					TryCreateEntity
					(
						placeableComponent.Blueprint.Name,
						placeableComponent.Position,
						placeableComponent.Orientation,
						playingAreaComponent.Terrain,
						playingAreaEntity.EntityFactory(),
						0
					)
				);

				return true;
			}
			else return false;
		}
	}
}
