﻿/***
 * game1666proto3: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;

namespace game1666proto3
{
	/// <summary>
	/// Represents a city in the game model.
	/// </summary>
	sealed class City : ICompositeModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		private readonly List<Building> m_buildings;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The mesh of the terrain on which the city is founded.
		/// </summary>
		public TerrainMesh TerrainMesh { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new city that is founded on the specified terrain.
		/// </summary>
		/// <param name="terrainMesh">The mesh of the terrain on which the city is founded.</param>
		public City(TerrainMesh terrainMesh)
		{
			this.TerrainMesh = terrainMesh;
			m_buildings = new List<Building>();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The building to add.</param>
		public void AddEntity(Building building)
		{
			m_buildings.Add(building);
		}

		/// <summary>
		/// Gets the sub-entities within the city.
		/// </summary>
		/// <returns>An IEnumerable of the sub-entities.</returns>
		public IEnumerable<IModelEntity> GetEntities()
		{
			foreach(var building in m_buildings)
			{
				yield return building;
			}
		}

		#endregion
	}
}
