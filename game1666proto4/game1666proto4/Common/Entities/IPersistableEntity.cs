/***
 * game1666proto4: IPersistableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can be persisted from one run to the next.
	/// </summary>
	interface IPersistableEntity
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the entity to XML.
		/// </summary>
		XElement SaveToXML();

		#endregion
	}
}
