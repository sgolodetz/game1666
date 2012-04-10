﻿/***
 * game1666: CompositeInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components.Interaction
{
	/// <summary>
	/// An instance of this class provides composite interaction behaviour to an entity.
	/// </summary>
	sealed class CompositeInteractionComponent : InteractionComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "CompositeInteraction"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite interaction component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public CompositeInteractionComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			foreach(IUIEntity child in Entity.Children)
			{
				if(ViewportContains(child.Viewport, state.X, state.Y))
				{
					var interactor = child.GetComponent<InteractionComponent>(InteractionComponent.StaticGroup);
					if(interactor != null)
					{
						interactor.OnMouseMoved(state);
					}
				}
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			foreach(IUIEntity child in Entity.Children)
			{
				if(ViewportContains(child.Viewport, state.X, state.Y))
				{
					var interactor = child.GetComponent<InteractionComponent>(InteractionComponent.StaticGroup);
					if(interactor != null)
					{
						interactor.OnMousePressed(state);
					}
				}
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not a viewport contains a specific point.
		/// </summary>
		/// <param name="viewport">The viewport.</param>
		/// <param name="x">The x coordinate of the point.</param>
		/// <param name="y">The y coordinate of the point.</param>
		/// <returns>true, if the viewport contains the point, or false otherwise.</returns>
		private static bool ViewportContains(Viewport viewport, int x, int y)
		{
			return viewport.Bounds.Left <= x && x < viewport.Bounds.Right &&
				   viewport.Bounds.Top <= y && y < viewport.Bounds.Bottom;
		}

		#endregion
	}
}
