/***
 * game1666: PlayViewer.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Interaction;
using game1666.GameUI.Entities.Components.Rendering;

namespace game1666.GameUI.Entities.Concrete
{
	/// <summary>
	/// An instance of this class can be used to view a playing area.
	/// </summary>
	sealed class PlayViewer : UIEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new play viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the viewer's XML representation.</param>
		public PlayViewer(XElement entityElt)
		:	base(entityElt)
		{
			new PlayInteractionComponent().AddToEntity(this);
			new PlayRenderingComponent().AddToEntity(this);
		}

		#endregion
	}
}
