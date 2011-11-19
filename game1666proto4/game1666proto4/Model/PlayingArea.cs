/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	abstract class PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly RoadNetwork m_roadNetwork;
		private readonly Terrain m_terrain;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public PlayingArea(Terrain terrain)
		{
			m_terrain = terrain;
			m_roadNetwork = new RoadNetwork();
		}

		#endregion
	}
}
