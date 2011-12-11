/***
 * game1666proto4: Renderer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Linq;
using Microsoft.Xna.Framework;
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
		public static GraphicsDevice GraphicsDevice		{ get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws an axis-aligned bounding box (AABB).
		/// </summary>
		/// <param name="bounds">The bounding box in question.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="colour">The colour to use for the bounding box.</param>
		public static void DrawBoundingBox(BoundingBox bounds, Effect effect, Color colour)
		{
			VertexPositionColor[] vertices = bounds.GetCorners().Select(v => new VertexPositionColor(v, colour)).ToArray();
			var indices = new short[]
			{
				0, 1,	1, 2,	2, 3,	3, 0,	// top
				4, 5,	5, 6,	6, 7,	7, 4,	// bottom
				0, 4,	1, 5,	2, 6,	3, 7	// connectors
			};
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Renderer.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length, indices, 0, indices.Length / 2);
			}
		}

		/// <summary>
		/// Draws a triangle list using vertices and indices from the specified buffers.
		/// </summary>
		/// <param name="vertexBuffer">The vertex buffer containing the triangles' vertices.</param>
		/// <param name="indexBuffer">The index buffer specifying how the vertices make up the triangles.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		public static void DrawTriangleList(VertexBuffer vertexBuffer, IndexBuffer indexBuffer, Effect effect)
		{
			Renderer.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			Renderer.GraphicsDevice.Indices = indexBuffer;
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				Renderer.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
			}
		}

		/// <summary>
		/// Sets the appropriate settings on the graphics device to prepare for 3D rendering.
		/// </summary>
		public static void Setup3D()
		{
			// Set up the depth stencil state.
			var depthStencilState = new DepthStencilState();
			depthStencilState.DepthBufferEnable = true;
			GraphicsDevice.DepthStencilState = depthStencilState;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.CullClockwiseFace;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Set up the first sampler state.
			var samplerState = new SamplerState();
			samplerState.AddressU = samplerState.AddressV = TextureAddressMode.Wrap;
			GraphicsDevice.SamplerStates[0] = samplerState;
		}

		#endregion
	}
}
