﻿/***
 * game1666proto4: ViewManager.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class manages the view hierarchy for the game.
	/// </summary>
	sealed class ViewManager : CompositeViewEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The different game views, e.g. City, World, etc.
		/// </summary>
		private IDictionary<string,View> m_views = new Dictionary<string,View>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a view manager from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the manager's XML representation.</param>
		public ViewManager(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a view to the view manager.
		/// </summary>
		/// <param name="view">The view.</param>
		public void AddEntity(View view)
		{
			m_views[view.Name] = view;
		}

		/// <summary>
		/// Adds an entity to the view manager based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Draws the current view.
		/// </summary>
		public override void Draw()
		{
			// TEMPORARY
			m_views["City"].Draw();
		}

		/// <summary>
		/// Gets an entity in the view manager by its (relative) path, e.g. "City".
		/// </summary>
		/// <param name="path">The path to the entity.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count != 0) return null;
			else return this;
		}

		#endregion
	}
}
