/***
 * game1666: CompositeControlRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of this class provides composite rendering behaviour to a UI control.
	/// Specifically, the rendering behaviour for controls with this component is to render
	/// all sub-controls contained within the control.
	/// </summary>
	class CompositeControlRenderingComponent : ControlRenderingComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "CompositeControlRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite control rendering component.
		/// </summary>
		protected CompositeControlRenderingComponent()
		{}

		/// <summary>
		/// Constructs a composite control rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public CompositeControlRenderingComponent(XElement componentElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the UI control of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			foreach(IUIEntity child in Entity.Children)
			{
				var renderer = child.GetComponent<ControlRenderingComponent>(ControlRenderingComponent.StaticGroup);
				if(renderer != null)
				{
					renderer.Draw();
				}
			}
		}

		#endregion
	}
}
