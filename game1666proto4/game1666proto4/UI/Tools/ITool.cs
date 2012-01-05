/***
 * game1666proto4: ITool.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// An instance of a derived class implementing this interface represents a tool that can used to interact with a playing area.
	/// </summary>
	interface ITool
	{
		/// <summary>
		/// A name relevant to the type of tool being used (e.g. for entity placement tools, this would specify the relevant blueprint).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		void OnMouseMoved(MouseState state);

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		void OnMousePressed(MouseState state);
	}
}
