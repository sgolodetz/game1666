/***
 * game1666: StateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components
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
		public override string Group { get { return UIEntityComponentGroups.STATE; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a state component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		protected StateComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion
	}
}
