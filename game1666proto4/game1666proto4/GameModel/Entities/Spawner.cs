/***
 * game1666proto4: Spawner.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.GameModel.FSMs;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a spawner that can be used to generate new walkers
	/// to help populate the world/city. Spawners are generally placed at the edge of the map.
	/// </summary>
	sealed class Spawner : Building
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a spawner directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the spawner.</param>
		/// <param name="initialStateID">The initial state of the spawner.</param>
		public Spawner(IDictionary<string,dynamic> properties, PlaceableEntityStateID initialStateID)
		:	base(properties, initialStateID)
		{}

		/// <summary>
		/// Constructs a spawner from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the house's XML representation.</param>
		public Spawner(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this spawner that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new Spawner(Properties, PlaceableEntityStateID.IN_CONSTRUCTION);
		}

		#endregion
	}
}
