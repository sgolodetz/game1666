/***
 * game1666proto4: View.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a particular game view, e.g. the City View.
	/// </summary>
	sealed class View : CompositeViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The view entities that together make up the view.
		/// </summary>
		private IDictionary<string,ViewEntity> m_children = new Dictionary<string,ViewEntity>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the view's XML representation.</param>
		public View(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a view entity (such as a playing area viewer) to the view.
		/// </summary>
		/// <param name="entity">The view entity.</param>
		public void AddEntity(ViewEntity entity)
		{
			m_children[entity.Name] = entity;
		}

		/// <summary>
		/// Adds an entity to the view based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Draws the view.
		/// </summary>
		public override void Draw()
		{
			foreach(ViewEntity entity in m_children.Values)
			{
				entity.Draw();
			}
		}

		/// <summary>
		/// Updates the view based on user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			foreach(ViewEntity entity in m_children.Values)
			{
				entity.Update(gameTime);
			}
		}

		#endregion
	}
}
