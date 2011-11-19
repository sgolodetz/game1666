﻿/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4
{
	sealed class City : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly List<IBuilding> m_buildings;

		#endregion

		//#################### PROPERTIES ####################
		#region

		public string Name { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public City(string name, Terrain terrain)
		:	base(terrain)
		{
			Name = name;
			m_buildings = new List<IBuilding>();
		}

		#endregion
	}
}