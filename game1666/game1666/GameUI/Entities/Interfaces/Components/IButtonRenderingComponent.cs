/***
 * game1666: IButtonRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.Entities;

namespace game1666.GameUI.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides rendering behaviour to a button.
	/// </summary>
	interface IButtonRenderingComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A hook that can be used to check whether or not the button should be drawn highlighted.
		/// </summary>
		Func<bool> IsHighlighted { set; }

		#endregion
	}
}
