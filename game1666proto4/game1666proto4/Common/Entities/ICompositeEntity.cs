/***
 * game1666proto4: ICompositeEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface can contain other entities.
	/// </summary>
	interface ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		IEnumerable<dynamic> Children { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the composite based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		void AddDynamicEntity(dynamic entity);

		#endregion
	}
}
