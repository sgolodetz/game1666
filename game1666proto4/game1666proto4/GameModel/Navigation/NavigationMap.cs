/***
 * game1666proto4: NavigationMap.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Entities;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel.Navigation
{
	/// <summary>
	/// An instance of this class represents a map that entities can use to navigate a terrain.
	/// </summary>
	sealed class NavigationMap
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The terrain over which entities will be navigating.
		/// </summary>
		public Terrain Terrain { private get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a road segment to the navigation map.
		/// </summary>
		/// <param name="roadSegment">The road segment.</param>
		public void AddEntity(RoadSegment roadSegment)
		{
			// TODO
		}

		#endregion
	}
}
