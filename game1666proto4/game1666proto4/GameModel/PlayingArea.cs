/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a playing area (e.g. the world or a city).
	/// </summary>
	abstract class PlayingArea : CompositeUpdatableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		public Terrain Terrain	{ get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a playing area directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the playing area.</param>
		public PlayingArea(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a playing area from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the playing area's XML representation.</param>
		public PlayingArea(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a terrain to the playing area (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			Terrain = terrain;
		}

		#endregion
	}
}
