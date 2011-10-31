/***
 * game1666proto3: ICompositeModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto3
{
	/// <summary>
	/// The interface for composite entities that are part of the game model, e.g. cities.
	/// </summary>
	interface ICompositeModelEntity : IModelEntity
	{
		/// <summary>
		/// Gets the sub-entities within the composite.
		/// </summary>
		/// <returns>An IEnumerable of the sub-entities.</returns>
		IEnumerable<IModelEntity> GetEntities();
	}
}
