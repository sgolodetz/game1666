/***
 * game1666proto4: Pathfinder.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of this class is used to find paths across a terrain.
	/// </summary>
	/// <typeparam name="PlaceableEntityType"></typeparam>
	sealed class Pathfinder<PlaceableEntityType> where PlaceableEntityType : class
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The occupancy map for the terrain over which we will be finding paths.
		/// </summary>
		private OccupancyMap<PlaceableEntityType> m_occupancyMap;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// The occupancy map for the terrain over which we will be finding paths.
		/// </summary>
		/// <param name="occupancyMap"></param>
		public Pathfinder(OccupancyMap<PlaceableEntityType> occupancyMap)
		{
			m_occupancyMap = occupancyMap;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Finds a path from the specified source to the nearest of the specified destinations over the terrain.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destinations">The destinations.</param>
		/// <returns>The path, as a list of points to traverse, or null if no path can be found.</returns>
		public List<Vector2> FindPath(Vector2 source, List<Vector2> destinations)
		{
			var sourceSquare = source.Discretize();
			var destinationSquares = destinations.Select(v => v.Discretize()).ToList();

			// TODO
			return null;
		}

		#endregion
	}
}
