/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666proto4.Common.Messages;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Messages;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a playing area in the game.
	/// </summary>
	sealed class PlayingArea : IPlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The message rules that have been registered by the playing area for the purpose of destructing entities.
		/// </summary>
		private readonly IDictionary<dynamic,MessageRule<dynamic>> m_destructionRules = new Dictionary<dynamic,MessageRule<dynamic>>();

		/// <summary>
		/// The mobile entities contained within the playing area.
		/// </summary>
		private readonly IDictionary<string,IMobileEntity> m_mobiles = new Dictionary<string,IMobileEntity>();

		/// <summary>
		/// The playing area's occupancy map.
		/// </summary>
		private readonly OccupancyMap m_occupancyMap = new OccupancyMap();

		/// <summary>
		/// The placeable entities contained within the playing area.
		/// </summary>
		private readonly IDictionary<string,IPlaceableEntity> m_placeables = new Dictionary<string,IPlaceableEntity>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entities contained within the playing area.
		/// </summary>
		public IEnumerable<dynamic> Children
		{
			get
			{
				foreach(IMobileEntity e in m_mobiles.Values)
				{
					yield return e;
				}

				foreach(IPlaceableEntity e in m_placeables.Values)
				{
					yield return e;
				}
			}
		}

		/// <summary>
		/// The mobile entities contained within the playing area.
		/// </summary>
		public IEnumerable<IMobileEntity> Mobiles { get { return m_mobiles.Values; } }

		/// <summary>
		/// The playing area's occupancy map.
		/// </summary>
		public OccupancyMap OccupancyMap { get { return m_occupancyMap; } }

		/// <summary>
		/// The placeable entities contained within the playing area.
		/// </summary>
		public IEnumerable<IPlaceableEntity> Placeables { get { return m_placeables.Values; } }

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		public Terrain Terrain	{ get; private set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the playing area based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a mobile entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddEntity(IMobileEntity entity)
		{
			m_mobiles.Add(entity.Name, entity);
			RegisterEntityDestructionRule(entity);
		}

		/// <summary>
		/// Adds a placeable entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddEntity(IPlaceableEntity entity)
		{
			m_placeables.Add(entity.Name, entity);
			RegisterEntityDestructionRule(entity);

			OccupancyMap.MarkOccupied(
				entity.PlacementStrategy.Place(
					Terrain,
					entity.Blueprint.Footprint,
					entity.Position,
					entity.Orientation
				),
				entity
			);
		}

		/// <summary>
		/// Adds a terrain to the playing area (note that there can only be one terrain).
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		public void AddEntity(Terrain terrain)
		{
			Terrain = terrain;
			OccupancyMap.Terrain = terrain;
		}

		/// <summary>
		/// Deletes an entity from the playing area based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public void DeleteDynamicEntity(dynamic entity)
		{
			DeleteEntity(entity);
		}

		/// <summary>
		/// Deletes a mobile entity from the playing area.
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public void DeleteEntity(IMobileEntity entity)
		{
			m_mobiles.Remove(entity.Name);
			m_destructionRules.Remove(entity);
		}

		/// <summary>
		/// Deletes a placeable entity from the playing area (provided it's destructible).
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public void DeleteEntity(IPlaceableEntity entity)
		{
			if(!entity.Destructible) return;

			m_placeables.Remove(entity.Name);
			m_destructionRules.Remove(entity);

			OccupancyMap.MarkOccupied(
				entity.PlacementStrategy.Place(
					Terrain,
					entity.Blueprint.Footprint,
					entity.Position,
					entity.Orientation
				),
				null
			);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Registers a message rule that responds to the destruction of an entity by deleting it from the playing area.
		/// </summary>
		/// <param name="entity">The entity in whose destruction we're interested.</param>
		private void RegisterEntityDestructionRule(dynamic entity)
		{
			m_destructionRules[entity] = MessageSystem.RegisterRule(
				MessageRuleFactory.FromSource(
					entity,
					new Action<EntityDestructionMessage>(msg => DeleteEntity(entity))
				)
			);
		}

		#endregion
	}
}
