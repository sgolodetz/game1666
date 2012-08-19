/***
 * game1666: CompositeRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this class provides composite rendering behaviour to a UI entity.
	/// Specifically, the rendering behaviour for entities with this component is to render
	/// all sub-entities contained within the entity.
	/// </summary>
	class CompositeRenderingComponent : RenderingComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "CompositeRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite rendering component.
		/// </summary>
		protected CompositeRenderingComponent()
		{}

		/// <summary>
		/// Constructs a composite rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		public CompositeRenderingComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the UI entity of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			foreach(IUIEntity child in Entity.Children)
			{
				var renderer = child.GetComponent<RenderingComponent>(UIEntityComponentGroups.RENDERING);
				if(renderer != null) renderer.Draw();
			}
		}

		#endregion
	}
}
