/***
 * game1666proto3: IViewEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto3
{
	/// <summary>
	/// The base interface for entities that are part of the game interface, e.g. the city viewer.
	/// </summary>
	interface IViewEntity
	{
		/// <summary>
		/// Draws the entity.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		/// <param name="landscapeTexture">The content manager containing any textures to use when drawing.</param>
		void Draw(GraphicsDevice graphics, ref BasicEffect basicEffect, ContentManager content);
	}
}
