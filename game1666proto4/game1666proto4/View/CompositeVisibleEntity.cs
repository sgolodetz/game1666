/***
 * game1666proto4: CompositeVisibleEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a composite of visible entities in the game, e.g. a sidebar viewer.
	/// </summary>
	abstract class CompositeVisibleEntity : CompositeEntity<IVisibleEntity>, IVisibleEntity
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
		/// Constructs a composite visible entity without any properties.
		/// </summary>
		public CompositeVisibleEntity()
		{}

		/// <summary>
		/// Constructs a composite visible entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeVisibleEntity(XElement entityElt)
		:	base(entityElt)
		{
			foreach(dynamic child in EntityUtil.LoadChildEntities(entityElt))
			{
				AddEntityDynamic(child);
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity.
		/// </summary>
		virtual public void Draw()
		{
			foreach(IVisibleEntity entity in Children)
			{
				entity.Draw();
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		virtual public void OnMousePressed(MouseState state)
		{
			foreach(IVisibleEntity entity in Children)
			{
				Viewport viewport = entity.Viewport;
				if(viewport.Bounds.Left <= state.X && state.X < viewport.Bounds.Right &&
				   viewport.Bounds.Top <= state.Y && state.Y < viewport.Bounds.Bottom)
				{
					entity.OnMousePressed(state);
				}
			}
		}

		/// <summary>
		/// Updates the entity based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		virtual public void Update(GameTime gameTime)
		{
			foreach(IVisibleEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion
	}
}
