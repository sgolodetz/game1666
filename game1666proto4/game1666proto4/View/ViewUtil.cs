/***
 * game1666proto4: ViewUtil.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4
{
	/// <summary>
	/// This class contains view-related utility methods.
	/// </summary>
	static class ViewUtil
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Parses the string representation of a viewport in order to construct the viewport itself.
		/// </summary>
		/// <param name="viewportSpecifier">The string representation of a viewport.</param>
		/// <returns>The viewport.</returns>
		public static Viewport ParseViewportSpecifier(string viewportSpecifier)
		{
			// TODO
			return Renderer.GraphicsDevice.Viewport;
		}

		#endregion
	}
}
