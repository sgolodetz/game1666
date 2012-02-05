/***
 * game1666proto4: MobileEntityBlueprint.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing a mobile entity.
	/// </summary>
	class MobileEntityBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The speed at which the entity's animation should be played (relative to the normal speed, i.e. 2.0 means twice normal speed).
		/// </summary>
		public float AnimationSpeed { get { return Properties["AnimationSpeed"]; } }

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
			Properties = EntityLoader.LoadProperties(blueprintElt);
		}

		#endregion
	}
}
