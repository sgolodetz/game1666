/***
 * game1666proto3: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666proto3
{
	enum EntityOrientation
	{
		RIGHT,
		UP,
		LEFT,
		DOWN
	}

	/// <summary>
	/// Represents a building in the game model.
	/// </summary>
	sealed class Building : IPlaceableModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Footprint m_footprint;
		private readonly EntityOrientation m_orientation;
		private readonly Vector3 m_position;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public Footprint Footprint { get { return m_footprint; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public Building(Footprint footprint, Vector3 position, EntityOrientation orientation)
		{
			m_footprint = footprint;
			m_position = position;
			m_orientation = orientation;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public bool ValidateFootprint(TerrainMesh terrainMesh)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
