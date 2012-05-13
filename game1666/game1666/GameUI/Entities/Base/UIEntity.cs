/***
 * game1666: UIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Base
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game's UI.
	/// </summary>
	sealed class UIEntity : Entity<IUIEntity>, IUIEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override IUIEntity Self { get { return this; } }

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get { return Properties["Viewport"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity with the specified archetype and viewport.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		/// <param name="viewport">The viewport of the entity.</param>
		public UIEntity(string name, string archetype, Viewport viewport)
		:	base(name, archetype)
		{
			Properties = new Dictionary<string,dynamic>
			{
				{ "Name", name },
				{ "Viewport", viewport }
			};
		}

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public UIEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
