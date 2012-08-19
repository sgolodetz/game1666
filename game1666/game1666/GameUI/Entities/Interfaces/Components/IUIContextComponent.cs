/***
 * game1666: IUIContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.GameModel.Entities.Base;

namespace game1666.GameUI.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides UI context objects, such as the
	/// world being viewed, to a UI entity tree. It is intended for use as a component of the root
	/// entity of such a tree, e.g. a game view.
	/// </summary>
	interface IUIContextComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The world that is being viewed by the entities in this UI entity tree.
		/// </summary>
		ModelEntity World { get; }

		#endregion
	}
}
