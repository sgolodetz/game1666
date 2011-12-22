/***
 * game1666proto4: EntityFootprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents the 'footprint' of an entity on a terrain.
	/// </summary>
	sealed class EntityFootprint : Entity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity footprint from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the footprint's XML representation.</param>
		public EntityFootprint(XElement entityElt)
		:	base(entityElt)
		{
			// TODO
		}

		#endregion
	}
}
