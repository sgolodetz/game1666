/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	abstract class PlayingArea
	{
		//#################### PROPERTIES ####################
		#region

		public RoadNetwork RoadNetwork	{ get; private set; }
		public Terrain Terrain			{ get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public PlayingArea(Terrain terrain)
		{
			this.Terrain = terrain;
			this.RoadNetwork = new RoadNetwork();
		}

		#endregion
	}
}
