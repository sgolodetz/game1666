/***
 * game1666proto4: SpawnerBlueprint.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for a spawner.
	/// </summary>
	sealed class SpawnerBlueprint : PlaceableEntityBlueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A map from the resources offered by the spawner to the corresponding types of entity that get spawned.
		/// </summary>
		public Dictionary<string,string> Offers { get { return Properties["Offers"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a spawner blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public SpawnerBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
