/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a city.
	/// </summary>
	sealed class City : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly List<Building> m_buildings = new List<Building>();	/// the buildings in the city

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a city with the specified name on the specified terrain.
		/// </summary>
		/// <param name="name">The name of the city.</param>
		/// <param name="terrain">The terrain on which the city stands.</param>
		public City(string name, Terrain terrain)
		:	base(terrain)
		{
			Properties["Name"] = name;
		}

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
		/// Adds an entity to the city based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
