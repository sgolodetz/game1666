/***
 * game1666: ButtonInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.UI;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components.Button
{
	/// <summary>
	/// An instance of this class provides interaction behaviour to a button.
	/// </summary>
	sealed class ButtonInteractionComponent : InteractionComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "ButtonInteraction"; } }

		#endregion

		//#################### EVENTS ####################
		#region

		/// <summary>
		/// Invoked when a mouse button is pressed.
		/// </summary>
		public event MouseEvent MousePressedHook = delegate {};

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			MousePressedHook(state);
		}

		#endregion
	}
}
