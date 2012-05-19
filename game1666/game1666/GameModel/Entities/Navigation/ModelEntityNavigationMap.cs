/***
 * game1666: ModelEntityNavigationMap.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameModel.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Navigation
{
	/// <summary>
	/// This class is used to provide a helpful typedef so that we don't have to keep typing out the long type name.
	/// </summary>
	sealed class ModelEntityNavigationMap : NavigationMap<IModelEntity,ModelEntityNavigationNode>
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a model entity navigation map for the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public ModelEntityNavigationMap(Terrain terrain)
		:	base(terrain)
		{}

		#endregion
	}
}
