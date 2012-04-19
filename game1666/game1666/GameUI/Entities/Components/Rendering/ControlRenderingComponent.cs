/***
 * game1666: ControlRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.GameUI.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of a class deriving from this one provides rendering behaviour to a UI control.
	/// </summary>
	abstract class ControlRenderingComponent : UIEntityComponent
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
		public static string StaticGroup { get { return "GameUI/Rendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a control rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected ControlRenderingComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Draws the UI control of which this component is a part.
		/// </summary>
		public abstract void Draw();

		#endregion
	}
}
