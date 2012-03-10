/***
 * game1666proto4: Renderer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4.Common.Graphics
{
	/// <summary>
	/// This class provides a global point of access for rendering.
	/// </summary>
	static class Renderer
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The graphics device for the game.
		/// </summary>
		private static GraphicsDevice s_graphicsDevice;

		/// <summary>
		/// The basic effect for rendering 2D lines.
		/// </summary>
		private static BasicEffect s_line2DEffect;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The content manager for the game.
		/// </summary>
		public static ContentManager Content			{ get; set; }

		/// <summary>
		/// The graphics device for the game.
		/// </summary>
		public static GraphicsDevice GraphicsDevice
		{
			get
			{
				return s_graphicsDevice;
			}

			set
			{
				s_graphicsDevice = value;
				s_line2DEffect = new BasicEffect(s_graphicsDevice);
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws an axis-aligned bounding box (AABB) in 2D.
		/// </summary>
		/// <param name="topLeft">The top-left of the bounding box in question.</param>
		/// <param name="bottomRight">The bottom-right of the bounding box in question.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="colour">The colour to use for the bounding box.</param>
		public static void DrawBoundingBox(Vector2 topLeft, Vector2 bottomRight, Effect effect, Color colour)
		{
			var vertices = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(topLeft.X, topLeft.Y, 0), colour),
				new VertexPositionColor(new Vector3(bottomRight.X, topLeft.Y, 0), colour),
				new VertexPositionColor(new Vector3(bottomRight.X, bottomRight.Y, 0), colour),
				new VertexPositionColor(new Vector3(topLeft.X, bottomRight.Y, 0), colour)
			};
			var indices = new short[] { 0, 1, 2, 3, 0 };
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length, indices, 0, indices.Length - 1);
			}
		}

		/// <summary>
		/// Draws an axis-aligned bounding box (AABB) in 3D.
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
				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length, indices, 0, indices.Length / 2);
			}
		}

		/// <summary>
		/// Draws a triple-circle representation of a 3D bounding sphere.
		/// </summary>
		/// <param name="boundingSphere">The bounding sphere.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="points">The number of vertices to use when drawing each of the three circles.</param>
		public static void DrawBoundingSphere(BoundingSphere boundingSphere, Effect effect, int points = 50)
		{
			Vector3 c = boundingSphere.Center;
			float r = boundingSphere.Radius;
			DrawEllipse(c, new Vector3(r, 0, 0), new Vector3(0, r, 0), effect, Color.Red, points);
			DrawEllipse(c, new Vector3(r, 0, 0), new Vector3(0, 0, r), effect, Color.Lime, points);
			DrawEllipse(c, new Vector3(0, r, 0), new Vector3(0, 0, r), effect, Color.Blue, points);
		}

		/// <summary>
		/// Draws an ellipse in 3D.
		/// </summary>
		/// <param name="centre">The centre of the ellipse.</param>
		/// <param name="uAxis">The major axis of the ellipse.</param>
		/// <param name="vAxis">The minor axis of the ellipse.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="colour">The colour to use for the ellipse.</param>
		/// <param name="points">The number of vertices to use when drawing the ellipse.</param>
		public static void DrawEllipse(Vector3 centre, Vector3 uAxis, Vector3 vAxis, Effect effect, Color colour, int points = 50)
		{
			// Construct the vertex and index buffers required to draw the ellipse.
			var vertices = new VertexPositionColor[points];
			var indices = new short[points+1];
			float angle = 0f;
			float deltaAngle = MathHelper.TwoPi / points;
			for(int i = 0; i < points; ++i, angle +=deltaAngle)
			{
				vertices[i] = new VertexPositionColor(centre + uAxis * (float)Math.Cos(angle) + vAxis * (float)Math.Sin(angle), colour);
				indices[i] = (short)i;
			}
			indices[points] = 0;

			// Actually draw the ellipse.
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length, indices, 0, indices.Length - 1);
			}
		}

		/// <summary>
		/// Draws a 2D line.
		/// </summary>
		/// <param name="v1">One end of the line.</param>
		/// <param name="v2">The other end of the line.</param>
		/// <param name="effect">The effect to use when drawing.</param>
		/// <param name="colour">The colour to use for the line.</param>
		public static void DrawLine(Vector2 v1, Vector2 v2, Effect effect, Color colour)
		{
			var vertices = new VertexPositionColor[]
			{
				new VertexPositionColor(new Vector3(v1.X, v1.Y, 0), colour),
				new VertexPositionColor(new Vector3(v2.X, v2.Y, 0), colour)
			};
			foreach(EffectPass pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
			}
		}

		/// <summary>
		/// Draws a 3D model.
		/// </summary>
		/// <param name="model">The model to draw.</param>
		/// <param name="matWorld">The world matrix to use.</param>
		/// <param name="matView">The view matrix to use.</param>
		/// <param name="matProjection">The projection matrix to use.</param>
		/// <param name="alpha">The alpha value to use.</param>
		public static void DrawModel(Model model, Matrix matWorld, Matrix matView, Matrix matProjection, float alpha = 1f)
		{
			// Change the cull mode - the models that are exported from Blender require counter-clockwise culling.
			var oldRasterizerState = GraphicsDevice.RasterizerState;
			var newRasterizerState = new RasterizerState();
			newRasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
			GraphicsDevice.RasterizerState = newRasterizerState;

			// Create an effect for drawing the bounding spheres in the model (for debugging purposes only).
			/*var sphereEffect = new BasicEffect(GraphicsDevice);
			sphereEffect.World = matWorld;
			sphereEffect.View = matView;
			sphereEffect.Projection = matProjection;
			sphereEffect.VertexColorEnabled = true;*/

			foreach(ModelMesh mesh in model.Meshes)
			{
				foreach(BasicEffect effect in mesh.Effects)
				{
					effect.World = matWorld;
					effect.View = matView;
					effect.Projection = matProjection;
					effect.Alpha = alpha;
				}
				mesh.Draw();

				// Draw the bounding sphere of the mesh (for debugging purposes only).
				//DrawBoundingSphere(mesh.BoundingSphere, sphereEffect);
			}

			// Restore the previous rasterizer state.
			GraphicsDevice.RasterizerState = oldRasterizerState;
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
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
			}
		}

		/// <summary>
		/// Fills in and returns a suitable basic effect for drawing 2D lines.
		/// </summary>
		/// <param name="viewport">The viewport into which the lines will be drawn.</param>
		/// <returns>The basic effect.</returns>
		public static BasicEffect Line2DEffect(Viewport viewport)
		{
			s_line2DEffect.Projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, -1f, 1f);
			s_line2DEffect.View = Matrix.Identity;
			s_line2DEffect.World = Matrix.Identity;
			s_line2DEffect.VertexColorEnabled = true;
			return s_line2DEffect;
		}

		/// <summary>
		/// Sets the appropriate settings on the graphics device to prepare for 2D rendering.
		/// </summary>
		public static void Setup2D()
		{
			// Set up the depth stencil state.
			var depthStencilState = new DepthStencilState();
			depthStencilState.DepthBufferEnable = false;
			GraphicsDevice.DepthStencilState = depthStencilState;

			// Set up the rasterizer state.
			var rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;
			GraphicsDevice.RasterizerState = rasterizerState;

			// Set up the first sampler state.
			var samplerState = new SamplerState();
			samplerState.AddressU = samplerState.AddressV = TextureAddressMode.Wrap;
			GraphicsDevice.SamplerStates[0] = samplerState;
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
