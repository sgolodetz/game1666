/***
 * game1666: InteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components.Interaction
{
	/// <summary>
	/// An instance of a class deriving from this one provides interaction behaviour to an entity.
	/// </summary>
	abstract class InteractionComponent : UIEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameUI/Interaction"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an interaction component.
		/// </summary>
		protected InteractionComponent()
		{}

		/// <summary>
		/// Constructs an interaction component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected InteractionComponent(XElement componentElt)
		{}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public abstract void OnMouseMoved(MouseState state);

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public abstract void OnMousePressed(MouseState state);

		#endregion
	}
}
