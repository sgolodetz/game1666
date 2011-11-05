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

		private readonly EntityFootprint m_footprint;		/// the footprint of the entity
		private readonly EntityOrientation m_orientation;	/// the orientation of the entity
		private readonly Tuple<int,int> m_position;			/// the position of the entity's hotspot
		private readonly TerrainMesh m_terrainMesh;			/// the terrain on which the entity will stand

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new placeable model entity with the specified footprint, position and orientation.
		/// </summary>
		/// <param name="footprint">The footprint of the entity.</param>
		/// <param name="position">The position of the entity's hotspot.</param>
		/// <param name="orientation">The orientation of the entity.</param>
		/// <param name="terrainMesh">The terrain on which the entity will stand.</param>
		protected PlaceableModelEntity(EntityFootprint footprint, Tuple<int,int> position, EntityOrientation orientation, TerrainMesh terrainMesh)
		{
			m_footprint = footprint;
			m_orientation = orientation;
			m_terrainMesh = terrainMesh;
			m_position = position;
		}

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		public EntityFootprint Footprint		{ get { return m_footprint; } }
		public EntityOrientation Orientation	{ get { return m_orientation; } }
		public Tuple<int,int> Position			{ get { return m_position; } }

		#endregion

		//#################### PROTECTED PROPERTIES ####################
		#region

		protected TerrainMesh TerrainMesh		{ get { return m_terrainMesh; } }

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the entity could be validly placed on the terrain mesh, given its position and orientation.
		/// </summary>
		/// <returns>true, if the entity could be validly placed, or false otherwise</returns>
		abstract public bool ValidateFootprint();

		#endregion
	}
}
