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
		public Viewport Viewport
		{
			get			{ return Properties["Viewport"]; }
			private set	{ Properties["Viewport"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public UIEntity(XElement entityElt)
		:	base(entityElt, null)
		{}

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public UIEntity(XElement entityElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(entityElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Creates a UI entity based on the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype on which to base the entity.</param>
		/// <param name="viewport">The viewport of the entity.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		/// <returns>The entity, if the specified prototype exists, or null otherwise.</returns>
		public static UIEntity CreateFromPrototype(string prototypeName, Viewport viewport, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		{
			XElement prototypeEntity = EntityPrototypeManager.GetPrototypeEntity(prototypeName);
			if(prototypeEntity == null) return null;

			var entity = new UIEntity(prototypeEntity, fixedProperties);
			entity.Prototype = prototypeName;
			entity.Viewport = viewport;
			return entity;
		}

		#endregion
	}
}
