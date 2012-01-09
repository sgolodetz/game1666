/***
 * game1666proto4: IPlayingArea.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of a class implementing this interface represents a playing area in the game.
	/// </summary>
	interface IPlayingArea : ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		Terrain Terrain { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a placeable entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		void AddPlaceableEntity(IPlaceableEntity entity);

		#endregion
	}
}
