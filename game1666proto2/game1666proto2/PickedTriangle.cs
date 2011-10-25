/***
 * game1666proto2: PickedTriangle.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto2
{
	/// <summary>
	/// Used to represent a triangle that has been picked (in the raycasting sense) by the user.
	/// </summary>
	struct PickedTriangle
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Vector3 m_pickPoint;
		private readonly Triangle m_triangle;

		#endregion

		//#################### PUBLIC PROPERTIES ####################
		#region

		/// <summary>
		/// The actual point on the triangle picked by the user.
		/// </summary>
		public Vector3	PickPoint	{ get { return m_pickPoint; } }

		/// <summary>
		/// The triangle picked by the user.
		/// </summary>
		public Triangle	Triangle	{ get { return m_triangle; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new PickedTriangle.
		/// </summary>
		/// <param name="pickPoint">The actual point on the triangle picked by the user.</param>
		/// <param name="triangle">The triangle picked by the user.</param>
		public PickedTriangle(Vector3 pickPoint, Triangle triangle)
		{
			m_pickPoint = pickPoint;
			m_triangle = triangle;
		}

		#endregion
	}
}
