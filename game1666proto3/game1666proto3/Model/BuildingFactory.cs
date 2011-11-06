/***
 * game1666proto3: BuildingFactory.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	static class BuildingFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a new house with the specified position and orientation, provided it will
		/// stand fully on the terrain.
		/// </summary>
		/// <param name="position">The position of the house's hotspot on the terrain.</param>
		/// <param name="orientation">The orientation of the house.</param>
		/// <param name="terrainMesh">The terrain on which the building will stand.</param>
		/// <returns>The constructed building, if it will stand fully on the terrain, or null otherwise.</returns>
		public static Building CreateHouse(Vector2i position, EntityOrientation orientation, TerrainMesh terrainMesh)
		{
			var pattern = new int[,]
			{
				{ 1, 1 },
				{ 1, 0 }
			};

			try
			{
				return new Building(new EntityFootprint(pattern, new Vector2i(0, 0), orientation), position, terrainMesh);
			}
			catch(Exception)
			{
				return null;
			}
		}

		#endregion
	}
}
