/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	sealed class PlayingAreaViewer
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly PlayingArea m_playingArea;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public PlayingAreaViewer(PlayingArea playingArea)
		{
			m_playingArea = playingArea;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public void Draw()
		{
			// TODO
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		private void DrawTerrain()
		{
			Terrain terrain = m_playingArea.Terrain;
			// TODO
		}

		#endregion
	}
}
