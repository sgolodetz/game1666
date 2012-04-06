/***
 * game1666: MouseEventManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Input;

namespace game1666.UI
{
	delegate void MouseEvent(MouseState state);

	/// <summary>
	/// Manages mouse events, allowing the user to handle mouse input via an event-based rather than polling-based model.
	/// </summary>
	static class MouseEventManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Used to hold the current mouse state.
		/// </summary>
		private static MouseState s_currentState;

		/// <summary>
		/// Used to hold the previous mouse state.
		/// </summary>
		private static MouseState s_previousState;

		#endregion

		//#################### EVENTS ####################
		#region

		/// <summary>
		/// Invoked when the mouse moves.
		/// </summary>
		public static event MouseEvent OnMouseMoved = delegate {};

		/// <summary>
		/// Invoked when a mouse button is pressed.
		/// </summary>
		public static event MouseEvent OnMousePressed = delegate {};

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the current state of the mouse and calls event handlers as necessary.
		/// </summary>
		public static void Update()
		{
			// Update the previous and current states of the mouse.
			s_previousState = s_currentState;
			s_currentState = Mouse.GetState();

			if((s_currentState.LeftButton == ButtonState.Pressed && s_previousState.LeftButton == ButtonState.Released) ||
			   (s_currentState.RightButton == ButtonState.Pressed && s_previousState.RightButton == ButtonState.Released))
			{
				// If any of the mouse buttons are down, fire off a mouse pressed event.
				OnMousePressed(s_currentState);
			}
			else if(s_currentState.X != s_previousState.X || s_currentState.Y != s_previousState.Y)
			{
				// Otherwise, if the mouse has been moved, fire off a mouse moved event.
				OnMouseMoved(s_currentState);
			}
		}

		#endregion
	}
}
