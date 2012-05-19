/***
 * game1666: IButtonInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.Common.UI;

namespace game1666.GameUI.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides interaction behaviour to a button.
	/// </summary>
	interface IButtonInteractionComponent : IEntityComponent
	{
		//#################### EVENTS ####################
		#region

		/// <summary>
		/// Invoked when a mouse button is pressed.
		/// </summary>
		event MouseEvent MousePressedHook;

		#endregion
	}
}
