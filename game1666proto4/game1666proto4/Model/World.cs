/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	sealed class World : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly IDictionary<string,City> m_cities;

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
			XDocument doc = XDocument.Load(filename);
			return LoadFromXml(doc.Element("world"));
		}

		public static World LoadFromXml(XElement worldElt)
		{
			var world = new World(Terrain.LoadFromXml(worldElt.Element("terrain")));
			// TODO: Load entities.
			return world;
		}

		#endregion
	}
}
