/***
 * game1666proto4: EntityNavigationMap.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.GameModel.Navigation;

namespace game1666proto4.GameModel.Core
{
	/// <summary>
	/// This class is used to provide a helpful typedef so that we don't have to keep typing out the long type name.
	/// </summary>
	sealed class EntityNavigationMap : NavigationMap<IPlaceableEntity,EntityNavigationNode> {}
}
