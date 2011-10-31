/***
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

		private readonly TerrainMesh m_terrainMesh;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public TerrainMesh TerrainMesh
		{
			get
			{
				return m_terrainMesh;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="terrainMesh">The mesh of the terrain on which the city is founded.</param>
		public City(TerrainMesh terrainMesh)
		{
			m_terrainMesh = terrainMesh;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Gets the sub-entities within the city.
		/// </summary>
		/// <returns>An IEnumerable of the sub-entities.</returns>
		public IEnumerable<IModelEntity> GetEntities()
		{
			// TODO
			throw new NotImplementedException();
		}

		#endregion
	}
}
