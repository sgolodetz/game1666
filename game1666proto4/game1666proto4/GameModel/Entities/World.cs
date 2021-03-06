﻿/***
 * game1666proto4: World.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Core;
using game1666proto4.GameModel.Lifetime;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a game world.
	/// </summary>
	sealed class World : ICompositeEntity, INamedEntity, IPersistableEntity, IPlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

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
		/// The player's home city.
		/// </summary>
		public string HomeCity { get { return m_properties["HomeCity"]; } }

		/// <summary>
		/// The mobile entities contained within the world.
		/// </summary>
		public IEnumerable<IMobileEntity> Mobiles { get { return m_playingArea.Mobiles; } }

		/// <summary>
		/// The name of the world (not relevant).
		/// </summary>
		public string Name { get { return null; } }

		/// <summary>
		/// The world's navigation map.
		/// </summary>
		public EntityNavigationMap NavigationMap { get { return m_playingArea.NavigationMap; } }

		/// <summary>
		/// The parent of the world.
		/// </summary>
		public INamedEntity Parent { get; set; }

		/// <summary>
		/// The persistable entities contained within the world.
		/// </summary>
		public IEnumerable<IPersistableEntity> Persistables { get { return m_playingArea.Persistables; } }

		/// <summary>
		/// The placeable entities contained within the world.
		/// </summary>
		public IEnumerable<IPlaceableEntity> Placeables { get { return m_playingArea.Placeables; } }

		/// <summary>
		/// The world's terrain.
		/// </summary>
		public Terrain Terrain { get { return m_playingArea.Terrain; } }

		/// <summary>
		/// The updateable entities contained within the world.
		/// </summary>
		public IEnumerable<IUpdateableEntity> Updateables { get { return m_playingArea.Updateables; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a world from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the world's XML representation.</param>
		public World(XElement entityElt)
		{
			m_properties = EntityPersister.LoadProperties(entityElt);
			EntityPersister.LoadAndAddChildEntities(this, entityElt);
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
		/// Adds a mobile entity to the world.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddEntity(IMobileEntity entity)
		{
			m_playingArea.AddEntity(entity);
			entity.Parent = this;
			EntityUtil.RegisterEntityDestructionRule(entity, this);
		}

		/// <summary>
		/// Adds a placeable entity to the world.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddEntity(IPlaceableEntity entity)
		{
			m_playingArea.AddEntity(entity);
			entity.Parent = this;
			EntityUtil.RegisterEntityDestructionRule(entity, this);
			EntityUtil.RegisterEntitySpawnRule(entity, this);
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
		/// Deletes a mobile entity from the world.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeleteEntity(IMobileEntity entity)
		{
			m_playingArea.DeleteEntity(entity);
			entity.Parent = null;
		}

		/// <summary>
		/// Deletes a placeable entity from the world (provided that it's destructible).
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void DeleteEntity(IPlaceableEntity entity)
		{
			if(entity.Destructible)
			{
				m_playingArea.DeleteEntity(entity);
				entity.Parent = null;
			}
		}

		/// <summary>
		/// Gets a named entity directly contained within the world.
		/// </summary>
		/// <param name="name">The name of the entity to look up.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public INamedEntity GetEntityByName(string name)
		{
			return name == "city:Home" ? GetEntityByName(HomeCity) : m_playingArea.GetEntityByName(name);
		}

		/// <summary>
		/// Checks whether or not an entity can be validly placed on the terrain,
		/// bearing in mind its footprint, position and orientation.
		/// </summary>
		/// <param name="entity">The entity to be checked.</param>
		/// <returns>true, if the entity can be validly placed, or false otherwise.</returns>
		public bool IsValidlyPlaced(IPlaceableEntity entity)
		{
			return m_playingArea.IsValidlyPlaced(entity);
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
		/// Saves the world to an XML file.
		/// </summary>
		/// <param name="filename">The name of the XML file.</param>
		public void SaveToFile(string filename)
		{
			new XDocument(SaveToXML()).Save(filename);
		}

		/// <summary>
		/// Saves the world to XML.
		/// </summary>
		/// <returns>An XML representation of the world.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, m_properties);
			EntityPersister.SaveChildEntities(entityElt, Persistables);
			return entityElt;
		}

		/// <summary>
		/// Updates the world based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			m_playingArea.Update(gameTime);
			EntityDestructionManager.FlushQueue();
		}

		#endregion
	}
}
