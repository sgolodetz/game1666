﻿/***
 * game1666proto3: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// Represents a building in the game model.
	/// </summary>
	sealed class Building : PlaceableModelEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified footprint and position on the terrain.
		/// </summary>
		/// <param name="footprint">The footprint of the building.</param>
		/// <param name="position">The position of the building's hotspot.</param>
		/// <param name="terrainMesh">The terrain on which the building will stand.</param>
		public Building(EntityFootprint footprint, Vector2i position, TerrainMesh terrainMesh)
		:	base(footprint, position, terrainMesh)
		{
			ConstructBuffers(2f);
		}

		#endregion
	}
}
