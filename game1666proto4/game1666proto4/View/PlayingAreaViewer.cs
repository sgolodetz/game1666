/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;

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
			DrawTerrain();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		private void DrawTerrain()
		{
			BasicEffect basicEffect = Renderer.DefaultBasicEffect.Clone() as BasicEffect;
			basicEffect.Texture = Renderer.Content.Load<Texture2D>("landscape");
			basicEffect.TextureEnabled = true;
			Renderer.DrawTriangleList(m_playingArea.Terrain.VertexBuffer, m_playingArea.Terrain.IndexBuffer, basicEffect);
		}

		#endregion
	}
}
