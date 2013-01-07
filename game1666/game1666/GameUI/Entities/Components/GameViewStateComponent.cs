/***
 * game1666: GameViewStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameUI.Tools;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this component is used to share state between the components that together make up a game view.
	/// </summary>
	sealed class GameViewStateComponent : StateComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "GameViewState"; } }

		/// <summary>
		/// The currently active tool (e.g. an entity placement tool), or null if no tool is active.
		/// </summary>
		public ITool Tool { get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a game view state component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public GameViewStateComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion
	}
}
