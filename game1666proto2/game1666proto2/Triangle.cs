/***
 * game1666proto2: Triangle.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto2
{
	sealed class Triangle
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private Vector3 m_normal;
		private Vector3[] m_vertices;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a triangle with the specified three vertices.
		/// </summary>
		/// <param name="v0">The first vertex.</param>
		/// <param name="v1">The second vertex.</param>
		/// <param name="v2">The third vertex.</param>
		public Triangle(Vector3 v0, Vector3 v1, Vector3 v2)
		{
			m_vertices = new Vector3[]
			{
				v0, v1, v2
			};

			CalculateNormal();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Calculates the normal of the triangle (note that we assume that it's a valid triangle for this prototype).
		/// </summary>
		private void CalculateNormal()
		{
			Vector3 a = m_vertices[1] - m_vertices[0];
			Vector3 b = m_vertices[2] - m_vertices[0];
			m_normal = Vector3.Normalize(Vector3.Cross(a, b));
		}

		#endregion
	}
}
