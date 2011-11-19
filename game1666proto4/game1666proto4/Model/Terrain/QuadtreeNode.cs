/***
 * game1666proto4: QuadtreeNode.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	sealed class QuadtreeNode
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private BoundingBox m_bounds;
		private QuadtreeNode[] m_children;
		private IDictionary<Vector2i,Triangle[]> m_triangles;

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public Vector2i? PickGridSquare(Ray ray)
		{
			// TODO
			throw new NotImplementedException();
		}

		#endregion
	}
}
