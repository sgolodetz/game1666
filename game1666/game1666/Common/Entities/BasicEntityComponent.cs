/***
 * game1666: BasicEntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class deriving from this one represents a component that can form part of a basic entity.
	/// </summary>
	abstract class BasicEntityComponent : EntityComponent<IBasicEntity>
	{
		/// <summary>
		/// Constructs a blank component.
		/// </summary>
		protected BasicEntityComponent()
		:	base()
		{}

		/// <summary>
		/// Constructs a component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		protected BasicEntityComponent(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected BasicEntityComponent(XElement componentElt)
		:	base(componentElt)
		{}
	}
}
