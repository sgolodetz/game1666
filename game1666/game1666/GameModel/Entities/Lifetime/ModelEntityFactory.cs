/***
 * game1666: ModelEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Lifetime
{
	/// <summary>
	/// An instance of this class can be used to construct model entities.
	/// </summary>
	sealed class ModelEntityFactory : IModelEntityFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a model entity based on the specified archetype and properties.
		/// </summary>
		/// <param name="archetype">The archetype of the model entity (e.g. Village).</param>
		/// <param name="properties">The properties of the various components of the entity.</param>
		/// <returns>The constructed entity.</returns>
		public IModelEntity MakeEntity(string archetype, IDictionary<string,dynamic> properties)
		{
			// TODO
			return null;
		}

		#endregion
	}
}
