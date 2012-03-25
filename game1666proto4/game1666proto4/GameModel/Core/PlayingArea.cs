/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Terrains;
using game1666proto4.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Core
{
	/// <summary>
	/// An instance of this class represents a playing area in the game.
	/// </summary>
	sealed class PlayingArea : IPlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The resource matchmaker for entities within the playing area.
		/// </summary>
		private readonly ResourceMatchmaker m_matchmaker = new ResourceMatchmaker();

		/// <summary>
		/// The mobile entities contained within the playing area.
		/// </summary>
		private readonly IDictionary<string,IMobileEntity> m_mobiles = new Dictionary<string,IMobileEntity>();

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		private readonly EntityNavigationMap m_navigationMap = new EntityNavigationMap();

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
		private IEnumerable<dynamic> Children
		{
			get
			{
				yield return Terrain;
				foreach(IPlaceableEntity e in m_placeables.Values)	{ yield return e; }
				foreach(IMobileEntity e in m_mobiles.Values)		{ yield return e; }
			}
		}

		/// <summary>
		/// The mobile entities contained within the playing area.
		/// </summary>
		public IEnumerable<IMobileEntity> Mobiles { get { return m_mobiles.Values; } }

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		public EntityNavigationMap NavigationMap { get { return m_navigationMap; } }

		/// <summary>
		/// The persistable entities contained within the playing area.
		/// </summary>
		public IEnumerable<IPersistableEntity> Persistables
		{
			get
			{
				return Children.Where(c => c as IPersistableEntity != null).Cast<IPersistableEntity>();
			}
		}

		/// <summary>
		/// The placeable entities contained within the playing area.
		/// </summary>
		public IEnumerable<IPlaceableEntity> Placeables { get { return m_placeables.Values; } }

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		public Terrain Terrain	{ get; private set; }

		/// <summary>
		/// The updateable entities contained within the playing area.
		/// </summary>
		public IEnumerable<IUpdateableEntity> Updateables
		{
			get
			{
				return Children.Where(c => c as IUpdateableEntity != null).Cast<IUpdateableEntity>();
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the playing area based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a mobile entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddEntity(IMobileEntity entity)
		{
			m_mobiles.Add(entity.Name, entity);

			entity.Matchmaker = m_matchmaker;
			entity.NavigationMap = NavigationMap;
		}

		/// <summary>
		/// Adds a placeable entity to the playing area.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void AddEntity(IPlaceableEntity entity)
		{
			m_placeables.Add(entity.Name, entity);

			entity.Altitude = Terrain.DetermineAverageAltitude(entity.Position);
			entity.Matchmaker = m_matchmaker;

			NavigationMap.MarkOccupied
			(
				entity.PlacementStrategy.Place
				(
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
			NavigationMap.Terrain = terrain;

			foreach(IPlaceableEntity entity in Placeables)
			{
				entity.Altitude = terrain.DetermineAverageAltitude(entity.Position);
			}
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
		}

		/// <summary>
		/// Deletes a placeable entity from the playing area (provided it's destructible).
		/// </summary>
		/// <param name="entity">The entity to delete.</param>
		public void DeleteEntity(IPlaceableEntity entity)
		{
			if(!entity.Destructible) return;

			m_placeables.Remove(entity.Name);

			NavigationMap.MarkOccupied
			(
				entity.PlacementStrategy.Place
				(
					Terrain,
					entity.Blueprint.Footprint,
					entity.Position,
					entity.Orientation
				),
				null
			);
		}

		/// <summary>
		/// Gets a named entity directly contained within the playing area.
		/// </summary>
		/// <param name="name">The name of the entity to look up.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public INamedEntity GetEntityByName(string name)
		{
			IPlaceableEntity placeable;
			if(m_placeables.TryGetValue(name, out placeable))
			{
				return placeable;
			}

			IMobileEntity mobile;
			if(m_mobiles.TryGetValue(name, out mobile))
			{
				return mobile;
			}

			return null;
		}

		/// <summary>
		/// Checks whether or not an entity can be validly placed on the terrain,
		/// bearing in mind its footprint, position and orientation.
		/// </summary>
		/// <param name="entity">The entity to be checked.</param>
		/// <returns>true, if the entity can be validly placed, or false otherwise.</returns>
		public bool IsValidlyPlaced(IPlaceableEntity entity)
		{
			// Step 1:	Check that the entity occupies one or more grid squares, and that all the grid squares it does occupy are empty.
			IEnumerable<Vector2i> gridSquares = entity.PlacementStrategy.Place
			(
				Terrain,
				entity.Blueprint.Footprint,
				entity.Position,
				entity.Orientation
			);

			if(gridSquares == null || !gridSquares.Any() || NavigationMap.AreOccupied(gridSquares))
			{
				return false;
			}

			// Step 2:	Check that there are currently no mobile entities in the grid squares that the entity would occupy.
			//			Note that this isn't an especially efficient way of going about this, but it will do for now.
			//			A better approach would involve keeping track of which mobile entities are in which grid squares,
			//			and then checking per-grid square rather than per-entity.
			var gridSquareSet = new HashSet<Vector2i>(gridSquares);
			foreach(IMobileEntity mobile in Mobiles)
			{
				if(gridSquareSet.Contains(mobile.Position.ToVector2i()))
				{
					return false;
				}
			}

			// If we didn't find any problems, then the entity is validly placed.
			return true;
		}

		/// <summary>
		/// Updates the playing area based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			foreach(IUpdateableEntity entity in Updateables)
			{
				entity.Update(gameTime);
			}

			m_matchmaker.Match();
		}

		#endregion
	}
}
