/***
 * game1666proto4: Pathfinder.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
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
		/// Finds a path from the specified source to the specified destination over the terrain.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destination">The destination.</param>
		/// <returns>The path, as a list of points to traverse.</returns>
		public List<Vector2> FindPath(Vector2 source, Vector2 destination)
		{
			// TODO
			return null;
		}

		#endregion
	}
}
