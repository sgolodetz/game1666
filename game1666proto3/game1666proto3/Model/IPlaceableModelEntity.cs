/***
 * game1666proto3: IPlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto3
{
	/// <summary>
	/// The interface for model entities that can be placed on the terrain.
	/// </summary>
	interface IPlaceableModelEntity : IModelEntity
	{
		Footprint Footprint { get; }
	}
}
