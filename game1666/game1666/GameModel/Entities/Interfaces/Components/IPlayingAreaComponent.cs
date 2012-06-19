/***
 * game1666: IPlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.Entities;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Interfaces.Components
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
		/// The entrances to the playing area.
		/// </summary>
		List<Vector2i> Entrances { get; }

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		INavigationMap<ModelEntity> NavigationMap { get; }

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		Terrain Terrain { get; }

		#endregion
	}
}
