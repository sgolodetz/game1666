/***
 * game1666: RenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of a class deriving from this one provides rendering behaviour to a UI entity.
	/// </summary>
	abstract class RenderingComponent : UIEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return UIEntityComponentGroups.RENDERING; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		protected RenderingComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Draws the UI entity of which this component is a part.
		/// </summary>
		public abstract void Draw();

		#endregion
	}
}
