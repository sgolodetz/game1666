/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a game world.
	/// </summary>
	sealed class World : IPlayingArea, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The cities in the game world.
		/// </summary>
		private readonly IDictionary<string,City> m_cities = new Dictionary<string,City>();

		/// <summary>
		/// The properties of the world.
		/// </summary>
		private IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the world.
		/// </summary>
		public IEnumerable<dynamic> Children { get { return m_cities.Values; } }

		/// <summary>
		/// The cities in the world.
		/// </summary>
		public IEnumerable<City> Cities { get { return m_cities.Values; } }

		/// <summary>
		/// The player's home city.
		/// </summary>
		public string HomeCity { get { return m_properties["HomeCity"]; } }

		/// <summary>
		/// The world's terrain.
		/// </summary>
		public Terrain Terrain	{ get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a world from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the world's XML representation.</param>
		public World(XElement entityElt)
		{
			m_properties = EntityLoader.LoadProperties(entityElt);
			EntityLoader.LoadAndAddChildEntities(this, entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the world based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a city to the world.
		/// </summary>
		/// <param name="city">The city.</param>
		public void AddEntity(City city)
		{
			m_cities[city.Name] = city;
		}

		/// <summary>
		/// Adds a terrain to the world (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			Terrain = terrain;
		}

		/// <summary>
		/// Adds a placeable entity to the world.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddPlaceableEntity(IPlaceableEntity entity)
		{
			AddDynamicEntity(entity);
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
		/// Gets an entity in the world by its (relative) path, e.g. "city:Stuartopolis".
		/// </summary>
		/// <param name="path">The path to the entity.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count != 0)
			{
				string first = path.Dequeue();
				if(first.StartsWith("city:"))
				{
					City city = first == "city:Home" ? GetCity(HomeCity) : GetCity(first.Substring("city:".Length));
					return city != null ? city.GetEntityByPath(path) : null;
				}
				else return null;
			}
			else return this;
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

		/// <summary>
		/// Updates the world based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			foreach(IUpdateableEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion
	}
}
