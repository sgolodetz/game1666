/***
 * game1666: StateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components.Common
{
	/// <summary>
	/// An instance of a class deriving from this one manages the state for a UI entity.
	/// </summary>
	abstract class StateComponent : UIEntityComponent
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
		public static string StaticGroup { get { return "GameUI/State"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a state component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected StateComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion
	}
}
