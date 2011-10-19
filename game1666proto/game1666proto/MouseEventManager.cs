/***
 * game1666proto: MouseManager.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Input;

namespace game1666proto
{
	delegate void MousePressedEvent(MouseState state);

	/// <summary>
	/// Manages mouse events, allowing the user to handle mouse input via an event-based rather than polling-based model.
	/// </summary>
	static class MouseEventManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private static MouseState s_currentState;
		private static MouseState s_previousState;

		#endregion

		//#################### EVENTS ####################
		#region

		public static event MousePressedEvent OnMousePressed = delegate {};

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the current state of the mouse and calls event handlers as necessary.
		/// </summary>
		public static void Update()
		{
			s_previousState = s_currentState;
			s_currentState = Mouse.GetState();

			// If any of the mouse buttons are down, fire off a mouse pressed event.
			if((s_currentState.LeftButton == ButtonState.Pressed && s_previousState.LeftButton == ButtonState.Released) ||
			   (s_currentState.RightButton == ButtonState.Pressed && s_previousState.RightButton == ButtonState.Released))
			{
				OnMousePressed(s_currentState);
			}
		}

		#endregion
	}
}
