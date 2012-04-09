/***
 * game1666: CompositeRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Linq;
using System.Xml.Linq;
using game1666.Common.Entities;

namespace game1666.GameUI.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of this class provides composite rendering behaviour to an entity.
	/// Specifically, the rendering behaviour for entities with this component is to
	/// render all children contained within the entity.
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
		/// Constructs a composite rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public CompositeRenderingComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			foreach(UIEntity child in Entity.Children.Cast<UIEntity>())
			{
				var renderer = child.GetComponent<RenderingComponent>(RenderingComponent.StaticGroup);
				if(renderer != null)
				{
					renderer.Draw();
				}
			}
		}

		#endregion
	}
}
