/***
 * game1666proto4: GameViewState.cs
 * Copyright 2011. All rights reserved.
 ***/

using game1666proto4.UI.Tools;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class is used to share state between the visible entities that together make up a game view.
	/// </summary>
	sealed class GameViewState
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The currently active tool (e.g. an entity placement tool), or null if no tool is active.
		/// </summary>
		public ITool Tool { get; set; }

		#endregion
	}
}
