/***
 * game1666proto4: Camera.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
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
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The position of the camera.
		/// </summary>
		public Vector3 Position { get; private set; }

		/// <summary>
		/// A vector pointing in the direction faced by the camera.
		/// </summary>
		public Vector3 N { get; private set; }

		/// <summary>
		/// A vector pointing to the left of the camera.
		/// </summary>
		public Vector3 U { get; private set; }

		/// <summary>
		/// A vector pointing to the top of the camera.
		/// </summary>
		public Vector3 V { get; private set; }

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
			Position = position;

			N = look;
			N.Normalize();

			V = up;
			V.Normalize();

			U = Vector3.Cross(V, N);
			U.Normalize();
		}

		#endregion
	}
}
