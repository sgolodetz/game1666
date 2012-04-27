/***
 * game1666: ButtonControl.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components.Button
{
	/// <summary>
	/// An instance of this class represents a clickable button control.
	/// </summary>
	sealed class ButtonControl : UIEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a button control.
		/// </summary>
		/// <param name="textureName">The name of the texture to use when drawing the button.</param>
		/// <param name="viewport">The viewport of the button.</param>
		public ButtonControl(string textureName, Viewport viewport)
		:	base(Guid.NewGuid().ToString(), "Button", viewport)
		{
			new ButtonInteractionComponent().AddToEntity(this);
			new ButtonRenderingComponent(textureName).AddToEntity(this);
		}

		#endregion
	}
}
