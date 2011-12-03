/***
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
	sealed class ViewManager : CompositeEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private IDictionary<string,View> m_views = new Dictionary<string,View>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="entityElt"></param>
		public ViewManager(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="view"></param>
		public void AddEntity(View view)
		{
			m_views[view.Name] = view;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="entity"></param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
