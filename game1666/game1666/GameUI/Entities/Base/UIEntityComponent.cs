/***
 * game1666: UIEntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;

namespace game1666.GameUI.Entities.Base
{
	/// <summary>
	/// An instance of a class deriving from this one represents a component that can form part of a UI entity.
	/// </summary>
	abstract class UIEntityComponent : EntityComponent<IUIEntity>
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blank component.
		/// </summary>
		protected UIEntityComponent()
		:	base()
		{}

		/// <summary>
		/// Constructs a component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		protected UIEntityComponent(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		protected UIEntityComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion
	}
}
