/***
 * game1666: IPersistableObject.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.Common.Persistence
{
	/// <summary>
	/// An instance of a class implementing this interface represents an object that can be persisted to XML.
	/// </summary>
	interface IPersistableObject
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the object to XML.
		/// </summary>
		/// <returns>An XML representation of the object.</returns>
		XElement SaveToXML();

		#endregion
	}
}
