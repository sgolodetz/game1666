/***
 * game1666proto4: ToolUtil.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// This class provides utility methods that can be used by the various tools.
	/// </summary>
	static class ToolUtil
	{
		/// <summary>
		/// Determine the 3D world space ray corresponding to the location of the user's mouse in the viewport.
		/// </summary>
		/// <param name="state">The current mouse state.</param>
		/// <param name="viewport">The viewport.</param>
		/// <param name="matProjection">The current projection matrix.</param>
		/// <param name="matView">The current view matrix.</param>
		/// <param name="matWorld">The current world matrix.</param>
		/// <returns>The ray.</returns>
		public static Ray DetermineMouseRay(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), matProjection, matView, matWorld);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), matProjection, matView, matWorld);

			// Find the ray (in world space) between them and return it.
			Vector3 dir = Vector3.Normalize(far - near);
			return new Ray(near, dir);
		}
	}
}
