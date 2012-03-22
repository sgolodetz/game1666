/***
 * game1666proto4: MovementStrategyFollowPath.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile
	/// entity to try and follow a specific path.
	/// </summary>
	sealed class MovementStrategyFollowPath : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The path that this movement strategy will cause its entity to try and follow.
		/// </summary>
		private Queue<Vector2> m_path;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { private get; set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		public EntityNavigationMap NavigationMap { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'follow path' movement strategy from a path.
		/// </summary>
		/// <param name="path">The path that this movement strategy will cause its entity to try and follow.</param>
		public MovementStrategyFollowPath(Queue<Vector2> path)
		{
			m_path = path;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tries to move the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>true, if the entity was able to move, or false otherwise.</returns>
		public bool Move(GameTime gameTime)
		{
			// Try and find the offset to the next waypoint towards which we should head (if any).
			// If there isn't one at the moment (e.g. because we're there already, or because the
			// pathfinding couldn't find a suitable route right now), exit.
			Vector2? potentialOffset = FindNextWaypointOffset();
			if(potentialOffset == null) return false;
			Vector2 offset = potentialOffset.Value;
			float offsetLength = offset.Length();

			// Having found the offset to the next node we're trying to visit, resize it to the appropriate length
			// based on the entity's movement speed and the elapsed time. Special care must be taken to "slow down"
			// when we're close to a path node - otherwise, we overshoot and end up oscillating from one side of it
			// to the other.
			MobileEntityBlueprint blueprint = BlueprintManager.GetBlueprint(EntityProperties["Blueprint"]);
			offset.Normalize();
			offset *= Math.Min(blueprint.MovementSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f, offsetLength);

			// Move the entity, setting its altitude based on the terrain height at its new position.
			EntityProperties["Position"] += offset;
			EntityProperties["Altitude"] = NavigationMap.Terrain.DetermineAltitude(EntityProperties["Position"]);

			// Set the entity's orientation based on the direction in which it is travelling.
			EntityProperties["Orientation"] = (Orientation8)((Math.Round(Math.Atan2(offset.Y, offset.X) / MathHelper.PiOver4) + 8) % 8);

			return true;
		}

		/// <summary>
		/// Saves the movement strategy to XML.
		/// </summary>
		/// <returns>An XML representation of the movement strategy.</returns>
		public XElement SaveToXML()
		{
			// Unimplemented, since 'follow path' movement strategies are not persisted between game sessions.
			throw new NotImplementedException();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Finds the offset to the next waypoint towards which we should head (if any).
		/// </summary>
		/// <returns>The offset to the next waypoint, if any, or null otherwise.</returns>
		private Vector2? FindNextWaypointOffset()
		{
			// If there are no nodes in the path, there is no next waypoint, so exit.
			if(m_path.Count == 0) return null;

			Vector2 pos = EntityProperties["Position"];

			// Check whether or not the current path is still usable.
			Vector2i curGridSquare = pos.ToVector2i();
			Vector2i nextGridSquare = m_path.Peek().ToVector2i();
			IPlaceableEntity curOccupier = NavigationMap.LookupEntity(curGridSquare);
			IPlaceableEntity nextOccupier = NavigationMap.LookupEntity(nextGridSquare);

			// There are various conditions that will make us keep following the path:
			// 1) The next grid square to be visited is empty.
			// 2) The next grid square is non-empty but contains a road segment (which is walkable).
			// 3) We're in a building and the current grid square is an entrance.
			// 4) The next grid square is the last one in the path and an entrance.
			bool usablePath = nextOccupier == null ||
							  nextOccupier is RoadSegment ||
							  curOccupier != null && curOccupier.Entrances.Contains(curGridSquare) ||
							  nextGridSquare == m_path.Last().ToVector2i() && nextOccupier.Entrances.Contains(nextGridSquare);

			// If none of the conditions are met, then exit: the containing movement strategy
			// will have to try and find a new path.
			if(!usablePath) return null;

			// If the path is usable, find the offset to the next node that needs visiting.
			Vector2 offset = m_path.Peek() - pos;
			while(offset.Length() <= Constants.EPSILON)
			{
				// If we've reached the first node in the path, dequeue it.
				m_path.Dequeue();

				// If we run out of nodes in the path, there is no next waypoint, so exit.
				if(m_path.Count == 0) return null;

				// Compute the offset to the next node in the path.
				offset = m_path.Peek() - pos;
			}

			return offset;
		}

		#endregion
	}
}
