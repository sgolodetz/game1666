/***
 * game1666proto4: EntityUtil.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// This class contains utility functions for loading entities from XML.
	/// </summary>
	static class EntityUtil
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Loads the children of an entity from XML.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <returns>The loaded children.</returns>
		public static List<dynamic> LoadChildEntities(XElement entityElt)
		{
			var children = new List<dynamic>();

			foreach(XElement childElt in entityElt.Elements("entity"))
			{
				// Look up the C# type of the child entity.
				string childTypename = "game1666proto4." + Convert.ToString(childElt.Attribute("type").Value);
				Type childType = Type.GetType(childTypename);

				if(childType != null)
				{
					// Construct the child entity and add it to the list.
					children.Add(Activator.CreateInstance(childType, childElt));
				}
				else
				{
					throw new InvalidOperationException("No such class: " + childTypename);
				}
			}

			return children;
		}

		/// <summary>
		/// Loads an entity's properties from XML.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <returns>The loaded properties.</returns>
		public static IDictionary<string,string> LoadProperties(XElement entityElt)
		{
			var properties = new Dictionary<string,string>();

			foreach(XElement propertyElt in entityElt.Elements("property"))
			{
				// Look up the name of the property.
				XAttribute nameAttribute = propertyElt.Attribute("name");

				// If the property element has a value attribute, use that. Otherwise, use the text enclosed within the element.
				XAttribute valueAttribute = propertyElt.Attribute("value");
				string value = valueAttribute != null ? valueAttribute.Value : propertyElt.Value.Trim().Replace(" ", "");

				// Provided the property is valid, store it for later use.
				if(nameAttribute != null && value != null)
				{
					properties[nameAttribute.Value] = value;
				}
			}

			return properties;
		}

		/// <summary>
		/// Parses the string representation of a Vector2i in order to construct the Vector2i itself.
		/// </summary>
		/// <param name="vectorSpecifier">The string representation of a Vector2i.</param>
		/// <returns>The Vector2i.</returns>
		public static Vector2i ParseVector2iSpecifier(string vectorSpecifier)
		{
			int[] values = vectorSpecifier
				.Split('(',',',')')
				.Where(v => !string.IsNullOrWhiteSpace(v))
				.Select(v => int.Parse(v.Trim(), CultureInfo.GetCultureInfo("en-GB")))
				.ToArray();

			if(values.Length == 2)
			{
				return new Vector2i(values[0], values[1]);
			}
			else throw new InvalidDataException("The vector specifier '" + vectorSpecifier + "' does not have the right number of components.");
		}

		#endregion
	}
}
