/***
 * game1666: INavigationMap.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.Maths;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Navigation
{
	/// <summary>
	/// An instance of a class implementing this interface handles navigation for a terrain.
	/// </summary>
	/// <typeparam name="PlaceableEntityType">The type of entity that gets placed on the terrain.</typeparam>
	interface INavigationMap<PlaceableEntityType> where PlaceableEntityType : class
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not any of the specified grid squares are occupied.
		/// </summary>
		/// <param name="gridSquares">The grid squares to check.</param>
		/// <returns>true, if any of the grid squares are occupied, or false otherwise.</returns>
		bool AreOccupied(IEnumerable<Vector2i> gridSquares);

		/// <summary>
		/// Finds a path from the specified source to the nearest of the specified destinations.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <param name="destinations">The destinations.</param>
		/// <param name="properties">A set of properties associated with the entity for which a
		/// path is to be found (can be null if irrelevant).</param>
		/// <returns>The path, as a list of points to traverse, or null if no path can be found.</returns>
		Queue<Vector2> FindPath(Vector2 source, IEnumerable<Vector2> destinations, IDictionary<string,dynamic> properties);

		/// <summary>
		/// Looks up the entity (if any) that occupies the specified grid square.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <returns>The entity occupying it, if any, or null otherwise.</returns>
		PlaceableEntityType LookupEntity(Vector2i gridSquare);

		/// <summary>
		/// Marks a set of grid squares with the entity they contain (if any).
		/// </summary>
		/// <param name="gridSquares">The grid squares to mark.</param>
		/// <param name="entity">The entity they contain (if any).</param>
		void MarkOccupied(IEnumerable<Vector2i> gridSquares, PlaceableEntityType entity);

		#endregion
	}
}
