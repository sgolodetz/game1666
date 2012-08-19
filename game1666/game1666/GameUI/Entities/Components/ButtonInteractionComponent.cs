/***
 * game1666: ButtonInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameUI.Entities.Interfaces.Components;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this class provides interaction behaviour to a button.
	/// </summary>
	sealed class ButtonInteractionComponent : InteractionComponent, IButtonInteractionComponent
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

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a button interaction component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		public ButtonInteractionComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

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
