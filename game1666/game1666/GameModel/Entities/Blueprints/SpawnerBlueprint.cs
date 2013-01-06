/***
 * game1666: SpawnerBlueprint.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for the spawner component of an entity.
	/// </summary>
	sealed class SpawnerBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A map from the resources offered by the spawner to the corresponding entity prototypes that get spawned.
		/// </summary>
		public IDictionary<string,string> Offers { get { return Properties["Offers"]; } }

		/// <summary>
		/// The delay between spawning consecutive entities (in milliseconds).
		/// </summary>
		public int SpawnDelay { get { return Properties["SpawnDelay"]; } }

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
