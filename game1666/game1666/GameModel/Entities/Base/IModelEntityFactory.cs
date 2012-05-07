/***
 * game1666: IModelEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666.GameModel.Entities.Base
{
	/// <summary>
	/// An instance of a class implementing this interface can be used to construct model entities.
	/// </summary>
	interface IModelEntityFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a model entity based on the specified archetype and properties.
		/// </summary>
		/// <param name="archetype">The archetype of the entity (e.g. Village).</param>
		/// <param name="properties">The properties of the various components of the entity.</param>
		/// <returns>The constructed entity.</returns>
		IModelEntity MakeEntity(string archetype, IDictionary<string,dynamic> properties);

		#endregion
	}
}
