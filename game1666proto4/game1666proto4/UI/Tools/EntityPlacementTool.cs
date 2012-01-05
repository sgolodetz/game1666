/***
 * game1666proto4: EntityPlacementTool.cs
 * Copyright 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// An instance of this class can be used to place entities in a playing area.
	/// </summary>
	sealed class EntityPlacementTool : ITool
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the blueprint specifying the kind of entity to place.
		/// </summary>
		public string Name { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity placement tool.
		/// </summary>
		/// <param name="name">The name of the blueprint specifying the kind of entity to place.</param>
		public EntityPlacementTool(string name)
		{
			Name = name;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public void OnMouseMoved(MouseState state)
		{
			// TODO
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public void OnMousePressed(MouseState state)
		{
			// TODO
		}

		#endregion
	}
}
