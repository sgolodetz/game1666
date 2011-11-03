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

		public static Building CreateHouse(Tuple<int,int> position, BuildingOrientation orientation)
		{
			var pattern = new int[,]
			{
				{ 1, 1 },
				{ 1, 0 }
			};

			return new Building(new BuildingFootprint(pattern, Tuple.Create(0, 0)), position, orientation);
		}

		#endregion
	}
}
