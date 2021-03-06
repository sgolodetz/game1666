﻿/***
 * game1666proto4: VisibleEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of a class derivd from this one represents an entity that will be drawn on the screen, e.g. a playing area viewer.
	/// </summary>
	abstract class VisibleEntity : IVisibleEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the entity.
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The properties of the entity.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; set; }

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get; protected set; }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		protected INamedEntity World { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a visible entity without any properties.
		/// </summary>
		/// <param name="world">The world that is being viewed.</param>
		protected VisibleEntity(INamedEntity world)
		:	base()
		{
			World = world;
		}

		/// <summary>
		/// Constructs a visible entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		/// <param name="world">The world that is being viewed.</param>
		protected VisibleEntity(IDictionary<string,dynamic> properties, INamedEntity world)
		{
			Properties = properties;
			World = world;
		}

		/// <summary>
		/// Constructs a visible entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		protected VisibleEntity(XElement entityElt, INamedEntity world)
		{
			Properties = EntityPersister.LoadProperties(entityElt);
			World = world;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity.
		/// </summary>
		public abstract void Draw();

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public virtual void OnMouseMoved(MouseState state)
		{
			// No-op by default
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public virtual void OnMousePressed(MouseState state)
		{
			// No-op by default
		}

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			// No-op by default
		}

		#endregion
	}
}
