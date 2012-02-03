/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2012. All rights reserved.
 ***/

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
		/// The entities contained within the playing area.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_children = new Dictionary<string,dynamic>();

		/// <summary>
		/// The message rules that have been registered by the playing area for the purpose of destructing entities.
		/// </summary>
		private readonly IDictionary<dynamic,MessageRule<dynamic>> m_destructionRules = new Dictionary<dynamic,MessageRule<dynamic>>();

		/// <summary>
		/// The playing area's occupancy map.
		/// </summary>
		private readonly OccupancyMap m_occupancyMap = new OccupancyMap();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entities contained within the playing area.
		/// </summary>
		public IEnumerable<dynamic> Children { get { return m_children.Values; } }

		/// <summary>
		/// The playing area's occupancy map.
		/// </summary>
		public OccupancyMap OccupancyMap { get { return m_occupancyMap; } }

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
			// TODO
		}

		/// <summary>
		/// Adds a placeable entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddEntity(IPlaceableEntity entity)
		{
			m_children.Add(entity.Name, entity);

			OccupancyMap.MarkOccupied(
				entity.PlacementStrategy.Place(
					Terrain,
					entity.Blueprint.Footprint,
					entity.Position,
					entity.Orientation
				),
				entity
			);

			m_destructionRules[entity] = MessageSystem.RegisterRule(
				MessageRuleFactory.FromSource(
					entity,
					(EntityDestructionMessage msg) => DeleteEntity(entity)
				)
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
		/// Deletes a placeable entity from the playing area (provided it's destructible).
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public void DeleteEntity(IPlaceableEntity entity)
		{
			if(!entity.Destructible) return;

			m_children.Remove(entity.Name);

			OccupancyMap.MarkOccupied(
				entity.PlacementStrategy.Place(
					Terrain,
					entity.Blueprint.Footprint,
					entity.Position,
					entity.Orientation
				),
				null
			);

			m_destructionRules.Remove(entity);
		}

		#endregion
	}
}
