/***
 * game1666: MobileBlueprint.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for the mobile component of an entity.
	/// </summary>
	sealed class MobileBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The speed at which the entity's animation should be played (relative to the normal speed, i.e. 2.0 means twice normal speed).
		/// </summary>
		public float AnimationSpeed { get { return Properties["AnimationSpeed"]; } }

		/// <summary>
		/// The maximum change in altitude (in units) that the entity can manage when crossing from one grid square to another.
		/// </summary>
		public float MaxAltitudeChange { get { return Properties["MaxAltitudeChange"]; } }

		/// <summary>
		/// The name of the 3D model for the type of entity.
		/// </summary>
		public string Model { get { return Properties["Model"]; } }

		/// <summary>
		/// The speed of the entity (in units/s).
		/// </summary>
		public float MovementSpeed { get { return Properties["MovementSpeed"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a mobile blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public MobileBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
