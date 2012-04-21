/***
 * game1666: PlacementBlueprint.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.PlacementStrategies;

namespace game1666.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for the placement component of an entity.
	/// </summary>
	sealed class PlacementBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The footprint for the type of entity.
		/// </summary>
		public Footprint Footprint { get; private set; }

		/// <summary>
		/// The name of the 3D model for the entity.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The placement strategy for the type of entity.
		/// </summary>
		public IPlacementStrategy PlacementStrategy { get; private set; }

		/// <summary>
		/// The overall time required to construct the entity (in milliseconds).
		/// </summary>
		public int TimeToConstruct { get { return Properties["TimeToConstruct"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placement blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public PlacementBlueprint(XElement blueprintElt)
		{
			Properties = PropertyPersister.LoadProperties(blueprintElt);

			ObjectPersister.LoadAndAddChildObjects
			(
				blueprintElt,
				new ChildObjectAdder
				{
					CanBeUsedFor = t => typeof(Footprint).IsAssignableFrom(t),
					AdditionalArguments = new object[] {},
					AddAction = o => Footprint = o
				}
			);

			// TODO: Get rid of this and add a loader for placement strategies.
			PlacementStrategy = new PlacementStrategyRequireFlatGround();
		}

		#endregion
	}
}
