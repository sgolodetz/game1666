/***
 * game1666: ModelEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components.External;

namespace game1666.GameModel.Entities.Lifetime
{
	/// <summary>
	/// An instance of this class can be used to construct model entities.
	/// </summary>
	sealed class ModelEntityFactory : IModelEntityFactory
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A lookup table of functions to make entities, keyed by archetype.
		/// </summary>
		private static IDictionary<string,Func<IDictionary<string,dynamic>,IModelEntity>> s_entityMakers;

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Sets up the entity makers.
		/// </summary>
		static ModelEntityFactory()
		{
			s_entityMakers = new Dictionary<string,Func<IDictionary<string,dynamic>,IModelEntity>>();
			s_entityMakers.Add("House", MakeHouse);
		}

		#endregion

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
			return s_entityMakers[archetype](properties);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		private static IModelEntity MakeHouse(IDictionary<string,dynamic> properties)
		{
			IModelEntity house = new ModelEntity(Guid.NewGuid().ToString(), "House");
			new PlaceableComponent
			(
				new Dictionary<string,dynamic>
				{
					// TODO
				}
			).AddToEntity(house);
			return house;
		}

		#endregion
	}
}
