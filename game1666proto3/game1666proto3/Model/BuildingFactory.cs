/***
 * game1666proto3: BuildingFactory.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using Microsoft.Xna.Framework;

namespace game1666proto3
{
	static class BuildingFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		public static Building CreateHouse(Vector3 position, EntityOrientation orientation)
		{
			var pattern = new int[,]
			{
				{ 1, 1 },
				{ 1, 0 }
			};

			return new Building(new Footprint(pattern, Tuple.Create(0, 0)), position, orientation);
		}

		#endregion
	}
}
