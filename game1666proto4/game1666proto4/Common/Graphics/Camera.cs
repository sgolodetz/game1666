/***
 * game1666proto4: Camera.cs
 * Copyright 2011. All rights reserved.
 ***/

using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;

namespace game1666proto4.Common.Graphics
{
	/// <summary>
	/// An instance of this class represents a camera for a 3D view.
	/// Cameras are defined with a position and three mutually-orthogonal
	/// axes, namely N (points in the direction faced by the camera),
	/// U (points to the left of the camera) and V (points to the top
	/// of the camera).
	/// </summary>
	sealed class Camera
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private Vector3 m_n;
		private Vector3 m_position;
		private Vector3 m_u;
		private Vector3 m_v;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The position of the camera.
		/// </summary>
		public Vector3 Position { get { return m_position; } }

		/// <summary>
		/// A vector pointing in the direction faced by the camera.
		/// </summary>
		public Vector3 N { get { return m_n; } }

		/// <summary>
		/// A vector pointing to the left of the camera.
		/// </summary>
		public Vector3 U { get { return m_u; } }

		/// <summary>
		/// A vector pointing to the top of the camera.
		/// </summary>
		public Vector3 V { get { return m_v; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new camera.
		/// </summary>
		/// <param name="position">The position of the camera.</param>
		/// <param name="look">A vector pointing in the direction faced by the camera.</param>
		/// <param name="up">The "up" direction for the camera.</param>
		public Camera(Vector3 position, Vector3 look, Vector3 up)
		{
			m_position = position;

			m_n = look;
			m_n.Normalize();

			m_v = up;
			m_v.Normalize();

			m_u = Vector3.Cross(m_v, m_n);
			m_u.Normalize();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Moves the camera by the specified displacement in the n direction.
		/// </summary>
		/// <param name="delta">The displacement by which to move.</param>
		public void MoveN(float delta)
		{
			m_position += delta * m_n;
		}

		/// <summary>
		/// Moves the camera by the specified displacement in the u direction.
		/// </summary>
		/// <param name="delta">The displacement by which to move.</param>
		public void MoveU(float delta)
		{
			m_position += delta * m_u;
		}

		/// <summary>
		/// Moves the camera by the specified displacement in the v direction.
		/// </summary>
		/// <param name="delta">The displacement by which to move.</param>
		public void MoveV(float delta)
		{
			m_position += delta * m_v;
		}

		/// <summary>
		/// Rotates the camera anticlockwise by the specified angle about the specified axis.
		/// </summary>
		/// <param name="axis">The axis about which to rotate.</param>
		/// <param name="angle">The angle by which to rotate (in radians).</param>
		public void Rotate(Vector3 axis, float angle)
		{
			// Note: We try and optimise things a little by observing that there's no point rotating an axis about itself.
			if(axis != m_n) m_n = MathUtil.RotateAboutAxis(m_n, angle, axis);
			if(axis != m_u) m_u = MathUtil.RotateAboutAxis(m_u, angle, axis);
			if(axis != m_v) m_v = MathUtil.RotateAboutAxis(m_v, angle, axis);
		}

		#endregion
	}
}
