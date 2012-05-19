/***
 * game1666: TaskFollowPath.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Tasks
{
	/// <summary>
	/// An instance of this class represents a task that causes a mobile entity to try and follow a specific path.
	/// </summary>
	sealed class TaskFollowPath : Task
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The entity.
		/// </summary>
		private readonly ModelEntity m_entity;

		/// <summary>
		/// The path that this task will cause the entity to try and follow.
		/// </summary>
		private readonly Queue<Vector2> m_path;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'follow path' task.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="path">The path that this task will cause the entity to try and follow.</param>
		public TaskFollowPath(ModelEntity entity, Queue<Vector2> path)
		{
			m_entity = entity;
			m_path = path;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Executes the task based on the amount of elapsed time, and returns its state after execution.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The state of the task after being executed for the specified amount of time.</returns>
		public override TaskState Execute(GameTime gameTime)
		{
			// Try and find the offset to the next waypoint towards which we should head (if any).
			// If there isn't one at the moment (e.g. because we're there already, or because the
			// pathfinder couldn't find a suitable route right now), exit.
			Vector2? potentialOffset = FindNextWaypointOffset();
			if(potentialOffset == null)
			{
				return m_path.Count == 0 ? TaskState.SUCCEEDED : TaskState.FAILED;
			}

			Vector2 offset = potentialOffset.Value;
			float offsetLength = offset.Length();

			// Having found the offset to the next node we're trying to visit, resize it to the appropriate length
			// based on the entity's movement speed and the elapsed time. Special care must be taken to "slow down"
			// when we're close to a path node - otherwise, we overshoot and end up oscillating from one side of it
			// to the other.
			var mobileComponent = m_entity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
			MobileBlueprint blueprint = mobileComponent.Blueprint;
			offset.Normalize();
			offset *= Math.Min(blueprint.MovementSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f, offsetLength);

			// Move the entity, setting its altitude based on the terrain height at its new position.
			var playingAreaComponent = m_entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			mobileComponent.Position += offset;
			mobileComponent.Altitude = playingAreaComponent.Terrain.DetermineAltitude(mobileComponent.Position);

			// Set the entity's orientation based on the direction in which it is travelling.
			mobileComponent.Orientation = (float)Math.Atan2(offset.Y, offset.X);

			return TaskState.IN_PROGRESS;
		}

		/// <summary>
		/// Saves the task to XML.
		/// </summary>
		/// <returns>An XML representation of the task.</returns>
		public override XElement SaveToXML()
		{
			// Unimplemented, since 'follow path' tasks are not persisted between game sessions.
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
			// If there are no nodes in the path, the entity's finished moving, so exit.
			if(m_path.Count == 0) return null;

			// Get the current position of the entity.
			var mobileComponent = m_entity.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
			Vector2 pos = mobileComponent.Position;

			// Check whether or not the current path is still usable.
			Vector2i curGridSquare = pos.ToVector2i();
			Vector2i nextGridSquare = m_path.Peek().ToVector2i();
			var playingAreaComponent = m_entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			ModelEntity curOccupier = playingAreaComponent.NavigationMap.LookupEntity(curGridSquare);
			ModelEntity nextOccupier = playingAreaComponent.NavigationMap.LookupEntity(nextGridSquare);

			// There are various conditions that will make us keep following the path:
			// 1) The next grid square to be visited is empty.
			// 2) The next grid square is non-empty but contains a road segment (which is walkable).
			// 3) We're in a building and the current grid square is an entrance.
			// 4) The next grid square is the last one in the path and an entrance.
			var curOccupierPlaceableComponent = curOccupier != null ? curOccupier.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL) : null;
			var nextOccupierPlaceableComponent = nextOccupier != null ? nextOccupier.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL) : null;
			bool usablePath = nextOccupier == null ||
							  nextOccupier.HasComponent(ModelEntityComponentGroups.EXTERNAL, "Traversable") ||
							  (curOccupier != null && curOccupierPlaceableComponent.Entrances.Contains(curGridSquare)) ||
							  (nextGridSquare == m_path.Last().ToVector2i() && nextOccupierPlaceableComponent.Entrances.Contains(nextGridSquare));

			// If none of the conditions are met, then exit: the containing movement strategy
			// will have to try and find a new path.
			if(!usablePath) return null;

			// If the path is usable, find the offset to the next node that needs visiting.
			Vector2 offset = m_path.Peek() - pos;
			while(offset.Length() <= Constants.EPSILON)
			{
				// If we've reached the first node in the path, dequeue it.
				m_path.Dequeue();

				// If we run out of nodes in the path, the entity's finished moving, so exit.
				if(m_path.Count == 0) return null;

				// Compute the offset to the next node in the path.
				offset = m_path.Peek() - pos;
			}

			return offset;
		}

		#endregion
	}
}
