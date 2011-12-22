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
using Microsoft.Xna.Framework.Graphics;

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
				string childTypename = typeof(EntityUtil).Namespace + "." + Convert.ToString(childElt.Attribute("type").Value);
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
		/// Loads an entity's typed properties from XML.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <returns>The loaded properties.</returns>
		public static IDictionary<string,dynamic> LoadProperties(XElement entityElt)
		{
			var properties = new Dictionary<string,dynamic>();

			// Set up the converters for the various supported types.
			var converters = new Dictionary<string,Func<string,dynamic>>();
			converters["float"] = s => Convert.ToSingle(s);
			converters["int"] = s => Convert.ToInt32(s);
			converters["List[float]"] = s => ParseFloatList(s);
			converters["string"] = s => s;
			converters["Vector2i"] = s => ParseVector2iSpecifier(s);
			converters["Viewport"] = s => ParseViewportSpecifier(s);

			foreach(XElement propertyElt in entityElt.Elements("property"))
			{
				// Look up the name of the property.
				XAttribute nameAttribute = propertyElt.Attribute("name");

				// If the property element has a type attribute, use it. Otherwise, use string as the default.
				XAttribute typeAttribute = propertyElt.Attribute("type");
				string type = typeAttribute != null ? typeAttribute.Value : "string";

				// If the property element has a value attribute, use that. Otherwise, use the text enclosed within the element.
				XAttribute valueAttribute = propertyElt.Attribute("value");
				string value = valueAttribute != null ? valueAttribute.Value : propertyElt.Value.Replace(" ", "");

				// Provided the property is valid, convert and store it for later use.
				if(nameAttribute != null && value != null)
				{
					properties[nameAttribute.Value] = converters[type](value);
				}
			}

			return properties;
		}

		/// <summary>
		/// Parses the string representation of a float list in order to construct the float list itself.
		/// </summary>
		/// <param name="listSpecifier">The string representation of a float list.</param>
		/// <returns>The float list.</returns>
		public static List<float> ParseFloatList(string listSpecifier)
		{
			return listSpecifier.Split(',').Select(s => Convert.ToSingle(s.Trim())).ToList();
		}

		/// <summary>
		/// Parses the string representation of a Vector2i in order to construct the Vector2i itself.
		/// </summary>
		/// <param name="vectorSpecifier">The string representation of a Vector2i.</param>
		/// <returns>The Vector2i.</returns>
		public static Vector2i ParseVector2iSpecifier(string vectorSpecifier)
		{
			int[] values = vectorSpecifier
				.Split(',')
				.Where(v => !string.IsNullOrWhiteSpace(v))
				.Select(v => Convert.ToInt32(v.Trim()))
				.ToArray();

			if(values.Length == 2)
			{
				return new Vector2i(values[0], values[1]);
			}
			else throw new InvalidDataException("The vector specifier '" + vectorSpecifier + "' does not have the right number of components.");
		}

		/// <summary>
		/// Parses the string representation of a viewport in order to construct the viewport itself.
		/// </summary>
		/// <param name="viewportSpecifier">The string representation of a viewport.</param>
		/// <returns>The viewport.</returns>
		public static Viewport ParseViewportSpecifier(string viewportSpecifier)
		{
			decimal[] values = viewportSpecifier
				.Split(',')
				.Select(v => Convert.ToDecimal(v.Trim()))
				.ToArray();

			if(values.Length == 4)
			{
				Viewport fullViewport = Renderer.GraphicsDevice.Viewport;
				return new Viewport
				(
					(int)(values[0] * fullViewport.Width),
					(int)(values[1] * fullViewport.Height),
					(int)(values[2] * fullViewport.Width),
					(int)(values[3] * fullViewport.Height)
				);
			}
			else throw new InvalidDataException("The viewport specifier '" + viewportSpecifier + "' does not have the right number of components.");
		}

		#endregion
	}
}
