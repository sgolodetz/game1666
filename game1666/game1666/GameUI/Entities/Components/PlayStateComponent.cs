/***
 * game1666: PlayStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameUI.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this class manages the state for a play viewer.
	/// </summary>
	sealed class PlayStateComponent : StateComponent, IPlayStateComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		public Camera Camera { get; private set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayState"; } }

		/// <summary>
		/// The current projection matrix.
		/// </summary>
		public Matrix ProjectionMatrix { get; private set; }

		/// <summary>
		/// The current view matrix.
		/// </summary>
		public Matrix ViewMatrix { get; private set; }

		/// <summary>
		/// The current world matrix.
		/// </summary>
		public Matrix WorldMatrix { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a play state component.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayStateComponent(XElement componentElt)
		:	base(componentElt)
		{
			Camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), Vector3.UnitZ);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Sets the world, view and projection matrices based on the current state of the camera.
		/// </summary>
		public void SetMatricesFromCamera()
		{
			ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)Entity.Viewport.Width / Entity.Viewport.Height, 0.1f, 1000.0f);
			ViewMatrix = Matrix.CreateLookAt(Camera.Position, Camera.Position + Camera.N, Camera.V);
			WorldMatrix = Matrix.Identity;
		}

		#endregion
	}
}
