/***
 * game1666proto4: IPlaceableEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

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

		#endregion
	}
}
