/***
 * game1666: ToolUtil.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using game1666.Common.Entities;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Extensions;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Entities.Interfaces.Context;
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
		/// <param name="prototypeName">The name of the prototype for the entity to create.</param>
		/// <param name="gridSquare">The terrain grid square on which to place the entity's hotspot.</param>
		/// <param name="orientation">The orientation of the entity.</param>
		/// <param name="terrain">The terrain on which the entity is to be placed.</param>
		/// <param name="percentComplete">The percentage of the entity that has been constructed.</param>
		/// <returns>The entity, if it can be created, or null otherwise.</returns>
		public static ModelEntity TryCreateEntity(string prototypeName, Vector2i? gridSquare, Orientation4 orientation, Terrain terrain, int percentComplete)
		{
			// If there's no grid square on which to place the entity's hotspot, early out.
			if(gridSquare == null) return null;

			// Construct the entity from its prototype, fixing some of the properties of its
			// external component to put it in the right place.
			ModelEntity entity = PrototypeManager.CreateModelEntityFromPrototype
			(
				prototypeName, new Dictionary<string,IDictionary<string,dynamic>>
				{
					{
						ModelEntityComponentGroups.EXTERNAL, new Dictionary<string,dynamic>
						{
							{ "Altitude", 0f },
							{ "ConstructionDone", 0 },
							{ "Orientation", orientation },
							{ "Position", gridSquare.Value },
							{ "State", "IN_CONSTRUCTION" }
						}
					}
				}
			);

			// After creating the entity, fix up any properties that could not be determined earlier.
			var placeableComponent = entity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			PlaceableBlueprint blueprint = placeableComponent.Blueprint;

			placeableComponent.ConstructionDone = blueprint.TimeToConstruct * percentComplete / 100;
			placeableComponent.State = percentComplete < 100 ? PlaceableComponentState.IN_CONSTRUCTION : PlaceableComponentState.OPERATING;

			// Attempt to determine the average altitude of the terrain beneath the entity's footprint.
			// Note that this will return null if the entity can't be validly placed.
			float? altitude = blueprint.Footprint.Rotated((int)orientation).DetermineAverageAltitude(gridSquare.Value, terrain);
			if(altitude != null)
			{
				placeableComponent.Altitude = altitude.Value;
				return entity;
			}
			else return null;
		}

		/// <summary>
		/// Tries to place an entity on the terrain of an entity that has a playing area,
		/// succeeding if and only if it is validly placed.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="playingAreaEntity">The entity that has the playing area.</param>
		/// <returns>true, if the placement succeeded, or false otherwise.</returns>
		public static bool TryPlaceEntity(ModelEntity entity, ModelEntity playingAreaEntity)
		{
			Contract.Requires(entity != null);
			Contract.Requires(playingAreaEntity != null);

			var placeableComponent = entity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);

			if(placeableComponent.IsValidlyPlaced(playingAreaEntity))
			{
				var playingAreaComponent = playingAreaEntity.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

				// Note: We know that the call to TryCreateEntity will always succeed, since the entity
				// that has been passed in was successfully created with almost identical properties.
				playingAreaEntity.AddChild
				(
					TryCreateEntity
					(
						entity.Prototype,
						placeableComponent.Position,
						placeableComponent.Orientation,
						playingAreaComponent.Terrain,
						0
					)
				);

				return true;
			}
			else return false;
		}
	}
}
