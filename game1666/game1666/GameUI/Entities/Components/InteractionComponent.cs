/***
 * game1666: InteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of a class deriving from this one provides interaction behaviour to a UI entity.
	/// </summary>
	abstract class InteractionComponent : UIEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return UIEntityComponentGroups.INTERACTION; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an interaction component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		protected InteractionComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public virtual void OnMouseMoved(MouseState state)
		{
			// No-op by default
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public virtual void OnMousePressed(MouseState state)
		{
			// No-op by default
		}

		#endregion
	}
}
