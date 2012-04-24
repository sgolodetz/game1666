/***
 * game1666: PlayStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework;

namespace game1666.GameUI.Entities.Components.Play
{
	/// <summary>
	/// An instance of this class manages the state for a play viewer.
	/// </summary>
	sealed class PlayStateComponent : StateComponent
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
	}
}
