/***
 * game1666proto4: IPlaceableEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be placed in a playing area.
	/// </summary>
	interface IPlaceableEntity : IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		float Altitude { get; }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		Blueprint Blueprint { get; }

		/// <summary>
		/// The finite state machine for the entity.
		/// </summary>
		FiniteStateMachine<EntityStateID> FSM { get; }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		Orientation4 Orientation { get; }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		Vector2i Position { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this entity that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		IPlaceableEntity CloneNew();

		/// <summary>
		/// Checks whether or not the entity can be validly placed on the specified terrain,
		/// bearing in mind its position and orientation.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>true, if it can be validly placed, or false otherwise</returns>
		bool IsValidlyPlaced(Terrain terrain);

		/// <summary>
		/// Attempts to place the entity on the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>A set of grid squares that the entity overlays, if it can be validly placed, or null otherwise</returns>
		IEnumerable<Vector2i> Place(Terrain terrain);

		#endregion
	}
}
