/***
 * game1666proto4: MobileEntityBlueprint.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for a mobile entity.
	/// </summary>
	sealed class MobileEntityBlueprint : Blueprint
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
		/// The speed of the entity (in units/s).
		/// </summary>
		public float MovementSpeed { get { return Properties["MovementSpeed"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a mobile entity blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public MobileEntityBlueprint(XElement blueprintElt)
		{
			Properties = EntityPersister.LoadProperties(blueprintElt);
		}

		#endregion
	}
}
