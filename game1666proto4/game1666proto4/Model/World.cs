/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;

namespace game1666proto4
{
	sealed class World : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private IDictionary<string,City> m_cities;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public World(Terrain terrain)
		:	base(terrain)
		{
			m_cities = new Dictionary<string,City>();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public void AddEntity(City city)
		{
			m_cities[city.Name] = city;
		}

		public static World LoadFromFile(string filename)
		{
			var world = new World(new Terrain());

			// TODO

			return world;
		}

		#endregion
	}
}
