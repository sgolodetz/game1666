/***
 * game1666proto4: CompositeViewEntity.cs
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
	/// An instance of this class represents a composite view entity in the game, e.g. a sidebar viewer.
	/// </summary>
	abstract class CompositeViewEntity : ViewEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		abstract protected IEnumerable<ViewEntity> Children { get; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite entity without any properties.
		/// </summary>
		public CompositeViewEntity()
		{}

		/// <summary>
		/// Constructs a composite entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public CompositeViewEntity(IDictionary<string,string> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a composite entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeViewEntity(XElement entityElt)
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
		/// Adds a child entity to this entity based on its dynamic type.
		/// </summary>
		/// <param name="entity">The child entity.</param>
		abstract public void AddEntityDynamic(dynamic entity);

		/// <summary>
		/// Draws the entity.
		/// </summary>
		public override void Draw()
		{
			foreach(ViewEntity entity in Children)
			{
				entity.Draw();
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point when the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			foreach(ViewEntity entity in Children)
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
		public override void Update(GameTime gameTime)
		{
			foreach(ViewEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion
	}
}
