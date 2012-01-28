/***
 * game1666proto4: IPlayingArea.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a playing area in the game.
	/// </summary>
	interface IPlayingArea : ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		NavigationMap NavigationMap { get; }

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		Terrain Terrain { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Deletes an entity from the playing area based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		void DeleteDynamicEntity(dynamic entity);

		#endregion
	}
}
