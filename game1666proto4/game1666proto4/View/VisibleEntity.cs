/***
 * game1666proto4: VisibleEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents an entity that will be drawn on the screen, e.g. a playing area viewer.
	/// </summary>
	abstract class VisibleEntity : Entity, IVisibleEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get; protected set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity without any properties.
		/// </summary>
		public VisibleEntity()
		:	base()
		{}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public VisibleEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity.
		/// </summary>
		abstract public void Draw();

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		virtual public void OnMousePressed(MouseState state)
		{
			// No-op by default
		}

		/// <summary>
		/// Updates the entity based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		virtual public void Update(GameTime gameTime)
		{
			// No-op by default
		}

		#endregion
	}
}
