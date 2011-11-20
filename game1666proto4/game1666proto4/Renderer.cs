/***
 * game1666proto4: Renderer.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// This class provides a global point of access for rendering.
	/// </summary>
	static class Renderer
	{
		//#################### PROPERTIES ####################
		#region

		public static ContentManager Content			{ get; set; }
		public static BasicEffect DefaultBasicEffect	{ get; set; }
		public static GraphicsDevice GraphicsDevice		{ get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws a triangle list using vertices and indices from the specified buffers.
		/// </summary>
		/// <param name="vertexBuffer">The vertex buffer containing the triangles' vertices.</param>
		/// <param name="indexBuffer">The index buffer specifying how the vertices make up the triangles.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		public static void DrawTriangleList(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, BasicEffect basicEffect)
		{
			Renderer.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			Renderer.GraphicsDevice.Indices = indexBuffer;
			foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Renderer.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
			}
		}

		#endregion
	}
}
