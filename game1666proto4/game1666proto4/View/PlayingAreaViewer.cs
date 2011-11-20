/***
 * game1666proto4: PlayingAreaViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class can be used to view a playing area.
	/// </summary>
	sealed class PlayingAreaViewer
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly PlayingArea m_playingArea;		/// the playing area to view

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a viewer for the specified playing area.
		/// </summary>
		/// <param name="playingArea">The playing area to view.</param>
		public PlayingAreaViewer(PlayingArea playingArea)
		{
			m_playingArea = playingArea;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the playing area.
		/// </summary>
		public void Draw()
		{
			DrawTerrain();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws the playing area's terrain.
		/// </summary>
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
