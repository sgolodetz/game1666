/***
 * game1666proto3: ICompositeViewEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto3
{
	/// <summary>
	/// The interface for composite entities that are part of the game interface, e.g. the city screen.
	/// </summary>
	interface ICompositeViewEntity : IViewEntity
	{
		/// <summary>
		/// Gets the sub-entities within the composite.
		/// </summary>
		/// <returns>An IEnumerable of the sub-entities.</returns>
		IEnumerable<IViewEntity> GetEntities();
	}
}
