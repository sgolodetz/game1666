/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Communication;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a game world.
	/// </summary>
	sealed class World : IPlayingArea, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The cities in the world.
		/// </summary>
		private readonly IDictionary<string,City> m_cities = new Dictionary<string,City>();

		/// <summary>
		/// The world's playing area.
		/// </summary>
		private readonly PlayingArea m_playingArea = new PlayingArea();

		/// <summary>
		/// The properties of the world.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the world.
		/// </summary>
		public IEnumerable<dynamic> Children { get { return m_playingArea.Children; } }

		/// <summary>
		/// The player's home city.
		/// </summary>
		public string HomeCity { get { return m_properties["HomeCity"]; } }

		/// <summary>
		/// The world's terrain.
		/// </summary>
		public Terrain Terrain	{ get { return m_playingArea.Terrain; } }

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
			m_cities.Add(city.Name, city);
			m_playingArea.AddEntity(city);
		}

		/// <summary>
		/// Adds a terrain to the world (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			m_playingArea.AddEntity(terrain);
		}

		/// <summary>
		/// Deletes an entity from the world based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeleteDynamicEntity(dynamic entity)
		{
			DeleteEntity(entity);
		}

		/// <summary>
		/// Deletes a city from the world (provided that it's destructible).
		/// </summary>
		/// <param name="city">The city.</param>
		public void DeleteEntity(City city)
		{
			if(city.Destructible)
			{
				m_cities.Remove(city.Name);
				m_playingArea.DeleteEntity(city);
			}
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
		/// Gets an entity in the world by its (relative) path, e.g. "world/city:Stuartopolis".
		/// </summary>
		/// <param name="pathString">The path, as a string.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(string pathString)
		{
			var path = new Queue<string>(pathString.Split('/').Where(s => !string.IsNullOrEmpty(s)));
			if(path.Count != 0)
			{
				switch(path.Dequeue())
				{
					case "world":
						return GetEntityByPath(path);
				}
			}
			return null;
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

			MessageSystem.ProcessMessageQueue();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Gets the city (if any) with the specified name.
		/// </summary>
		/// <param name="name">The name of the city.</param>
		/// <returns>The city, if it exists, or null otherwise.</returns>
		private City GetCity(string name)
		{
			City city;
			m_cities.TryGetValue(name, out city);
			return city;
		}

		#endregion
	}
}
