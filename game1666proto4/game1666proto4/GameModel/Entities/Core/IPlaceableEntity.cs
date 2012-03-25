/***
 * game1666proto4: IPlaceableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Entities.PlacementStrategies;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Matchmaking;

namespace game1666proto4.GameModel.Entities.Core
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be placed in a playing area.
	/// </summary>
	interface IPlaceableEntity : ICompositeEntity, INamedEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		float Altitude { get; set; }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		dynamic Blueprint { get; }

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		bool Destructible { get; }

		/// <summary>
		/// The entrances to the entity.
		/// </summary>
		IEnumerable<Vector2i> Entrances { get; }

		/// <summary>
		/// The finite state machine for the entity.
		/// </summary>
		PlaceableEntityFSM FSM { get; }

		/// <summary>
		/// The resource matchmaker for the entity's playing area.
		/// </summary>
		ResourceMatchmaker Matchmaker { set; }

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
