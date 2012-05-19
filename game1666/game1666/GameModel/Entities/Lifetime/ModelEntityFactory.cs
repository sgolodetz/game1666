/***
 * game1666: ModelEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components;

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
		private readonly static IDictionary<string,Func<IDictionary<string,dynamic>,ModelEntity>> s_entityMakers;

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Sets up the entity makers.
		/// </summary>
		static ModelEntityFactory()
		{
			s_entityMakers = new Dictionary<string,Func<IDictionary<string,dynamic>,ModelEntity>>();
			s_entityMakers.Add("House", MakeHouse);
			s_entityMakers.Add("RoadSegment", MakeRoadSegment);
			s_entityMakers.Add("Settlement", MakeSettlement);
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
		public ModelEntity MakeEntity(string archetype, IDictionary<string,dynamic> properties)
		{
			Func<IDictionary<string,dynamic>,ModelEntity> entityMaker = null;
			if(s_entityMakers.TryGetValue(archetype, out entityMaker))
			{
				return entityMaker(properties);
			}
			else return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Constructs a new house using the specified properties.
		/// </summary>
		/// <param name="properties">The properties of the various components of the house.</param>
		/// <returns>The constructed house.</returns>
		private static ModelEntity MakeHouse(IDictionary<string,dynamic> properties)
		{
			ModelEntity entity = new ModelEntity(Guid.NewGuid().ToString(), "House");
			new PlaceableComponent(properties).AddToEntity(entity);
			return entity;
		}

		/// <summary>
		/// Constructs a new road segment using the specified properties.
		/// </summary>
		/// <param name="properties">The properties of the various components of the road segment.</param>
		/// <returns>The constructed road segment.</returns>
		private static ModelEntity MakeRoadSegment(IDictionary<string,dynamic> properties)
		{
			ModelEntity entity = new ModelEntity(Guid.NewGuid().ToString(), "RoadSegment");
			new TraversableComponent(properties).AddToEntity(entity);
			return entity;
		}

		/// <summary>
		/// Constructs a new settlement using the specified properties.
		/// </summary>
		/// <param name="properties">The properties of the various components of the settlement.</param>
		/// <returns>The constructed settlement.</returns>
		private static ModelEntity MakeSettlement(IDictionary<string,dynamic> properties)
		{
			ModelEntity entity = new ModelEntity(Guid.NewGuid().ToString(), "Settlement");
			new PlaceableComponent(properties).AddToEntity(entity);
			// TODO: Add a playing area component here.
			return entity;
		}

		#endregion
	}
}
