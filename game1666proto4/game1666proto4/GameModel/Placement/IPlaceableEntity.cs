/***
 * game1666proto4: IPlaceableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;

namespace game1666proto4.GameModel.Placement
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be placed in a playing area.
	/// </summary>
	interface IPlaceableEntity : ICompositeEntity
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
		PlaceableEntityBlueprint Blueprint { get; }

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		bool Destructible { get; }

		/// <summary>
		/// The finite state machine for the entity.
		/// </summary>
		FiniteStateMachine<EntityStateID> FSM { get; }

		/// <summary>
		/// The name of the entity (must be unique within its playing area).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		Orientation4 Orientation { get; }

		/// <summary>
		/// The placement strategy for the entity.
		/// </summary>
		IPlacementStrategy PlacementStrategy { get; }

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
