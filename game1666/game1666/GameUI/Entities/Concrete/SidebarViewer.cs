/***
 * game1666: SidebarViewer.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Interaction;
using game1666.GameUI.Entities.Components.Rendering;

namespace game1666.GameUI.Entities.Concrete
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class SidebarViewer : UIEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new sidebar viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the viewer's XML representation.</param>
		public SidebarViewer(XElement entityElt)
		:	base(entityElt)
		{
			new CompositeInteractionComponent().AddToEntity(this);
			new SidebarRenderingComponent().AddToEntity(this);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after this entity is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			// TODO: Add buttons here.
			base.AfterAdd();
		}

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();
			// TODO: Remove buttons here.
		}

		#endregion
	}
}
