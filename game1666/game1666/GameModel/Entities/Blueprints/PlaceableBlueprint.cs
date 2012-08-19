/***
 * game1666: PlaceableBlueprint.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.PlacementStrategies;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for the placeable component of an entity.
	/// </summary>
	sealed class PlaceableBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The footprint for the type of entity.
		/// </summary>
		public Footprint Footprint { get; private set; }

		/// <summary>
		/// The name of the 3D model for the type of entity.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The placement strategy for the type of entity.
		/// </summary>
		public IPlacementStrategy PlacementStrategy { get; private set; }

		/// <summary>
		/// The overall time required to construct the type of entity (in milliseconds).
		/// </summary>
		public int TimeToConstruct { get { return Properties["TimeToConstruct"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placeable blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public PlaceableBlueprint(XElement blueprintElt)
		{
			Properties = PropertyPersister.LoadProperties(blueprintElt);
			Footprint = ObjectPersister.LoadChildObjects<Footprint>(blueprintElt).First();
			PlacementStrategy = ObjectPersister.LoadChildObjects<IPlacementStrategy>(blueprintElt).First();
		}

		#endregion
	}
}
