/***
 * game1666proto4: RenderingDetails.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// Acts as a global point of access for rendering details such as the graphics device, default basic effect and content.
	/// </summary>
	static class RenderingDetails
	{
		public static BasicEffect BasicEffect		{ get; set; }
		public static ContentManager Content		{ get; set; }
		public static GraphicsDevice GraphicsDevice	{ get; set; }
	}
}
