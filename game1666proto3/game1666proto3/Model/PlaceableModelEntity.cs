/***
 * game1666proto3: PlaceableModelEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	/// <summary>
	/// The base class for model entities that can be placed on the terrain.
	/// </summary>
	abstract class PlaceableModelEntity : IModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly EntityFootprint m_footprint;
		private readonly EntityOrientation m_orientation;
		private readonly Tuple<int,int> m_position;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public PlaceableModelEntity(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation)
		{
			m_footprint = footprint;
			m_position = position;
			m_orientation = orientation;
		}

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public EntityFootprint Footprint		{ get { return m_footprint; } }
		public EntityOrientation Orientation	{ get { return m_orientation; } }
		public Tuple<int,int> Position			{ get { return m_position; } }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		abstract public bool ValidateFootprint(TerrainMesh terrainMesh);

		#endregion
	}
}
