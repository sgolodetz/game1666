/***
 * game1666proto4: IPersistableEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.Common.Entities2
{
	/// <summary>
	/// An instance of a class implementing this interface can be {saved to/loaded from} XML.
	/// </summary>
	interface IPersistableEntity
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Loads the entity from XML.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		void LoadFromXML(XElement entityElt);

		/// <summary>
		/// Saves the entity to XML.
		/// </summary>
		/// <returns>The root element of the entity's XML representation.</returns>
		XElement SaveToXML();

		#endregion
	}
}
