/***
 * game1666: IPlayStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.UI;
using Microsoft.Xna.Framework;

namespace game1666.GameUI.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface manages the state for a play viewer.
	/// </summary>
	interface IPlayStateComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		Camera Camera { get; }

		/// <summary>
		/// The current projection matrix.
		/// </summary>
		Matrix ProjectionMatrix { get; }

		/// <summary>
		/// The current view matrix.
		/// </summary>
		Matrix ViewMatrix { get; }

		/// <summary>
		/// The current world matrix.
		/// </summary>
		Matrix WorldMatrix { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Sets the world, view and projection matrices based on the current state of the camera.
		/// </summary>
		void SetMatricesFromCamera();

		#endregion
	}
}
