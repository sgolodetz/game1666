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

		private readonly IDictionary<string,City> m_cities = new Dictionary<string,City>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public World(Terrain terrain)
		:	base(terrain)
		{}

		public World(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public void AddEntity(City city)
		{
			m_cities[city.Name] = city;
		}

		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		public static World LoadFromFile(string filename)
		{
			XDocument doc = XDocument.Load(filename);
			return new World(doc.Element("entity"));
		}

		#endregion
	}
}
