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
		void Draw();
	}
}
