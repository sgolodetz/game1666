/***
 * game1666proto4: ICompositeEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

namespace game1666proto4.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface can contain other entities.
	/// </summary>
	interface ICompositeEntity
	{
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
