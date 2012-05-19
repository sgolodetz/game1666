/***
 * game1666: IUIEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Interfaces.Context
{
	/// <summary>
	/// An instance of a class implementing this interface can be used to construct UI entities.
	/// </summary>
	interface IUIEntityFactory
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a UI entity based on the specified archetype, viewport and properties.
		/// </summary>
		/// <param name="archetype">The archetype of the entity (e.g. Button).</param>
		/// <param name="viewport">The viewport of the entity.</param>
		/// <param name="properties">The properties of the various components of the entity.</param>
		/// <returns>The constructed entity.</returns>
		IUIEntity MakeEntity(string archetype, Viewport viewport, IDictionary<string,dynamic> properties);

		#endregion
	}
}
