/***
 * game1666: IPlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.GameModel.Entities.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides playing area
	/// behaviour to an entity such as the world or a settlement. Playing areas
	/// have a terrain on which other entities can be placed or move around.
	/// </summary>
	interface IPlayingAreaComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		ModelEntityNavigationMap NavigationMap { get; }

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		Terrain Terrain { get; }

		#endregion
	}
}
