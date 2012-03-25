/***
 * game1666proto4: IPlacementStrategy.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Blueprints;

namespace game1666proto4.GameModel.Entities.Core
{
	/// <summary>
	/// An instance of a class implementing this interface represents a placement strategy for an entity
	/// (e.g. can only place on flat terrain).
	/// </summary>
	interface IPlacementStrategy
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Attempts to place an entity on the specified terrain, bearing in mind its footprint,
		/// position and orientation.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <param name="footprint">The entity's footprint.</param>
		/// <param name="position">The position of the entity's hotspot.</param>
		/// <param name="orientation">The entity's orientation.</param>
		/// <returns>A set of grid squares that the entity overlays, if it can be validly placed, or null otherwise.</returns>
		IEnumerable<Vector2i> Place(Terrain terrain, Footprint footprint, Vector2i position, Orientation4 orientation);

		#endregion
	}
}
