/***
 * game1666proto4: ViewUtil.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Globalization;
using System.IO;
using System.Linq;
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
			decimal[] values = viewportSpecifier.Split(',').Select(v => decimal.Parse(v.Trim(), CultureInfo.GetCultureInfo("en-GB"))).ToArray();
			if(values.Length == 4)
			{
				Viewport fullViewport = Renderer.GraphicsDevice.Viewport;
				return new Viewport
				(
					(int)(values[0] * fullViewport.Width),
					(int)(values[1] * fullViewport.Height),
					(int)(values[2] * fullViewport.Width),
					(int)(values[3] * fullViewport.Height)
				);
			}
			else throw new InvalidDataException("The viewport specifier '" + viewportSpecifier + "' does not have the right number of components.");
		}

		#endregion
	}
}
