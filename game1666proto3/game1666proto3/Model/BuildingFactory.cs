﻿/***
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
		/// Constructs a new house with the specified position and orientation.
		/// </summary>
		/// <param name="position">The position of the house's hotspot on the terrain.</param>
		/// <param name="orientation">The orientation of the house.</param>
		/// <returns></returns>
		public static Building CreateHouse(Tuple<int,int> position, EntityOrientation orientation)
		{
			var pattern = new int[,]
			{
				{ 1, 1 },
				{ 1, 0 }
			};

			return new Building(new EntityFootprint(pattern, Tuple.Create(0, 0)), position, orientation);
		}

		#endregion
	}
}
