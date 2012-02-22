/***
 * game1666proto4: MovementStrategyGoToPosition.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile entity to head towards a specific position.
	/// </summary>
	sealed class MovementStrategyGoToPosition : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The path that is currently being followed (if any).
		/// </summary>
		private Queue<Vector2> m_path;

		/// <summary>
		/// The properties of the movement strategy.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

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
		/// Constructs a 'go to position' movement strategy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the strategy's XML representation.</param>
		public MovementStrategyGoToPosition(XElement entityElt)
		{
			m_properties = EntityPersister.LoadProperties(entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Moves the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Move(GameTime gameTime)
		{
			// Try and find the offset to the next waypoint towards which we should head (if any).
			// If there isn't one at the moment (e.g. because we're there already, or because the
			// pathfinding couldn't find a suitable route right now), exit.
			Vector2? potentialOffset = FindNextWaypointOffset();
			if(potentialOffset == null) return;
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
			Vector3 pos = EntityProperties["Position"];
			pos.X += offset.X;
			pos.Y += offset.Y;
			pos.Z = NavigationMap.Terrain.DetermineAltitude(pos.XY());
			EntityProperties["Position"] = pos;

			// Set the entity's orientation based on the direction in which it is travelling.
			EntityProperties["Orientation"] = (Orientation8)((Math.Round(Math.Atan2(offset.Y, offset.X) / MathHelper.PiOver4) + 8) % 8);
		}

		/// <summary>
		/// Saves the movement strategy to XML.
		/// </summary>
		/// <returns>An XML representation of the movement strategy.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, m_properties);
			return entityElt;
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
			Vector3 pos = EntityProperties["Position"];

			// If there's no path currently in effect, or there's a placeable entity blocking the next
			// waypoint of the current path, try and find a new path. If we can't find one, exit.
			IPlaceableEntity occupier = m_path != null ? NavigationMap.LookupEntity(m_path.Peek().ToVector2i()) : null;
			if(m_path == null || (occupier != null && !(occupier is RoadSegment)))
			{
				m_path = NavigationMap.FindPath(pos.XY(), new List<Vector2> { m_properties["TargetPosition"] });
				if(m_path == null)
				{
					return null;
				}
			}

			// Find the offset to the next node in the path that needs visiting.
			Vector2 offset = m_path.Peek() - pos.XY();
			while(offset.Length() <= Constants.EPSILON)
			{
				// If we've reached the first node in the path, dequeue it.
				m_path.Dequeue();

				// If we run out of nodes in the path, reset the path to null and exit.
				if(m_path.Count == 0)
				{
					m_path = null;
					return null;
				}

				// Compute the offset to the next node in the path.
				offset = m_path.Peek() - pos.XY();
			}

			return offset;
		}

		#endregion
	}
}
