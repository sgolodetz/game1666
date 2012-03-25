/***
 * game1666proto4: IMobileEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can move (e.g. a walker).
	/// </summary>
	interface IMobileEntity : INamedEntity
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
		MobileEntityBlueprint Blueprint { get; set; }

		/// <summary>
		/// The resource matchmaker for the entity's playing area (note that this will change as it moves from one playing area to another).
		/// </summary>
		ResourceMatchmaker Matchmaker { set; }

		/// <summary>
		/// The movement strategy for the entity.
		/// </summary>
		IMovementStrategy MovementStrategy { set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		EntityNavigationMap NavigationMap { set; }

		/// <summary>
		/// The 2D 45-degree orientation of the entity.
		/// </summary>
		Orientation8 Orientation { get; }

		/// <summary>
		/// The position of the entity (relative to the origin of the containing entity).
		/// </summary>
		Vector2 Position { get; }

		#endregion
	}
}
