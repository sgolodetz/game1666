/***
 * game1666proto4: PlacementStrategyRequireFlatGround.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a placement strategy that enforces the condition that
	/// an entity can only be placed on flat terrain.
	/// </summary>
	class PlacementStrategyRequireFlatGround : PlacementStrategyBase
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
		/// <returns>A set of grid squares that the entity overlays, if it can be validly placed, or null otherwise</returns>
		public override IEnumerable<Vector2i> Place(Terrain terrain, Footprint footprint, Vector2i position, Orientation4 orientation)
		{
			footprint = footprint.Rotated((int)orientation);
			if(terrain.CalculateHeightRange(footprint.OverlaidGridSquares(position, terrain, false)) == 0f)
			{
				return footprint.OverlaidGridSquares(position, terrain, true);
			}
			else return null;
		}

		#endregion
	}
}
