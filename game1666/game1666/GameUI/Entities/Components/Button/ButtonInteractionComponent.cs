/***
 * game1666: ButtonInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

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

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			// TODO
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			// TODO
			System.Console.WriteLine("Clicked!");
		}

		#endregion
	}
}
