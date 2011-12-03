/***
 * game1666proto4: View.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// TODO
	/// </summary>
	sealed class View : CompositeEntity
	{
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
		/// Adds an entity to the view based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			// TODO
		}

		/// <summary>
		/// Draws the view.
		/// </summary>
		public void Draw()
		{
			// TODO
		}

		#endregion
	}
}
