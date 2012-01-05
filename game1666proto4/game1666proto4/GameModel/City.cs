﻿/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Terrains;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// An instance of this class represents a city.
	/// </summary>
	sealed class City : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		private readonly List<Building> m_buildings = new List<Building>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The buildings in the city.
		/// </summary>
		public IEnumerable<Building> Buildings { get { return m_buildings; } }

		/// <summary>
		/// The sub-entities contained within the city.
		/// </summary>
		protected override IEnumerable<IUpdateableEntity> Children { get { return m_buildings; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the city's XML representation.</param>
		public City(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a building to the city.
		/// </summary>
		/// <param name="building">The building.</param>
		public void AddEntity(Building building)
		{
			m_buildings.Add(building);
			Terrain.MarkOccupied(building.Place(Terrain));
		}

		/// <summary>
		/// Adds an entity to the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Gets an entity in the city by its (relative) path.
		/// </summary>
		/// <param name="path">The path to the entity.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count != 0) return null;
			else return this;
		}

		#endregion
	}
}