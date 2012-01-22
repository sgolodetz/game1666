/***
 * game1666proto4: CompositeVisibleEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class represents a composite of visible entities in the game, e.g. a sidebar viewer.
	/// </summary>
	abstract class CompositeVisibleEntity : VisibleEntity, ICompositeEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the entity.
		/// </summary>
		public abstract IEnumerable<dynamic> Children { get; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite visible entity without any properties.
		/// </summary>
		/// <param name="world">The world that is being viewed.</param>
		public CompositeVisibleEntity(World world)
		:	base(world)
		{}

		/// <summary>
		/// Constructs a composite visible entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		/// <param name="world">The world that is being viewed.</param>
		public CompositeVisibleEntity(IDictionary<string,dynamic> properties, World world)
		:	base(properties, world)
		{}

		/// <summary>
		/// Constructs a composite visible entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public CompositeVisibleEntity(XElement entityElt, World world)
		:	base(entityElt, world)
		{
			EntityLoader.LoadAndAddChildEntities(this, entityElt, world);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the composite based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public abstract void AddDynamicEntity(dynamic entity);

		/// <summary>
		/// Draws the entity.
		/// </summary>
		public override void Draw()
		{
			foreach(IVisibleEntity entity in Children)
			{
				entity.Draw();
			}
		}

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			foreach(IVisibleEntity entity in Children)
			{
				if(ViewportContains(entity.Viewport, state.X, state.Y))
				{
					entity.OnMouseMoved(state);
				}
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			foreach(IVisibleEntity entity in Children)
			{
				if(ViewportContains(entity.Viewport, state.X, state.Y))
				{
					entity.OnMousePressed(state);
				}
			}
		}

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			foreach(IVisibleEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Tests whether or not a viewport contains a specific point.
		/// </summary>
		/// <param name="viewport">The viewport.</param>
		/// <param name="x">The x coordinate of the point.</param>
		/// <param name="y">The y coordinate of the point.</param>
		/// <returns>true, if the viewport contains the point, or false otherwise.</returns>
		private static bool ViewportContains(Viewport viewport, int x, int y)
		{
			return viewport.Bounds.Left <= x && x < viewport.Bounds.Right &&
				   viewport.Bounds.Top <= y && y < viewport.Bounds.Bottom;
		}

		#endregion
	}
}
