/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a game world.
	/// </summary>
	sealed class World : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The cities in the game world.
		/// </summary>
		private readonly IDictionary<string,City> m_cities = new Dictionary<string,City>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a world based on the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public World(Terrain terrain)
		:	base(terrain)
		{}

		/// <summary>
		/// Constructs a world from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the world's XML representation.</param>
		public World(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a city to the world.
		/// </summary>
		/// <param name="city">The city.</param>
		public void AddEntity(City city)
		{
			m_cities[city.Name] = city;
		}

		/// <summary>
		/// Adds an entity to the world based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Gets the city (if any) with the specified name.
		/// </summary>
		/// <param name="name">The name of the city.</param>
		/// <returns>The city, if it exists, or null otherwise.</returns>
		public City GetCity(string name)
		{
			City city;
			m_cities.TryGetValue(name, out city);
			return city;
		}

		/// <summary>
		/// Loads a world from an XML file.
		/// </summary>
		/// <param name="filename">The name of the XML file.</param>
		/// <returns>The loaded world.</returns>
		public static World LoadFromFile(string filename)
		{
			XDocument doc = XDocument.Load(filename);
			return new World(doc.Element("entity"));
		}

		#endregion
	}
}
