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
			BasicEffect basicEffect = RenderingDetails.BasicEffect.Clone() as BasicEffect;
			basicEffect.Texture = RenderingDetails.Content.Load<Texture2D>("landscape");
			basicEffect.TextureEnabled = true;
			DrawTriangleList(m_playingArea.Terrain.VertexBuffer, m_playingArea.Terrain.IndexBuffer, basicEffect);
		}

		/// <summary>
		/// Draws a triangle list using vertices and indices from the specified buffers.
		/// </summary>
		/// <param name="vertexBuffer">The vertex buffer containing the triangles' vertices.</param>
		/// <param name="indexBuffer">The index buffer specifying how the vertices make up the triangles.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		private static void DrawTriangleList(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, BasicEffect basicEffect)
		{
			RenderingDetails.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			RenderingDetails.GraphicsDevice.Indices = indexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				RenderingDetails.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
			}
		}

		#endregion
	}
}
