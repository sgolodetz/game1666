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
		/// TODO
		/// </summary>
		/// <param name="entityElt"></param>
		public View(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="entity"></param>
		public override void AddEntityDynamic(dynamic entity)
		{
			// TODO
		}

		/// <summary>
		/// TODO
		/// </summary>
		public void Draw()
		{
			// TODO
		}

		#endregion
	}
}
