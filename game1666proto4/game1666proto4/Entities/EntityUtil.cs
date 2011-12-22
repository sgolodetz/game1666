/***
 * game1666proto4: EntityUtil.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

			// Set up the parsers for the various supported types.
			var parsers = new Dictionary<string,Func<string,dynamic>>();
			parsers["Array2D[int]"] = s => ParseArray2D(s, Convert.ToInt32);
			parsers["float"] = s => Convert.ToSingle(s);
			parsers["int"] = s => Convert.ToInt32(s);
			parsers["List[int]"] = s => ParseList(s, Convert.ToInt32);
			parsers["List[float]"] = s => ParseList(s, Convert.ToSingle);
			parsers["string"] = s => s;
			parsers["Vector2i"] = s => ParseVector2iSpecifier(s);
			parsers["Viewport"] = s => ParseViewportSpecifier(s);

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

				// Provided the property is valid, parse and store it for later use.
				if(nameAttribute != null && value != null)
				{
					properties[nameAttribute.Value] = parsers[type](value);
				}
			}

			return properties;
		}

		/// <summary>
		/// Parses the string representation of a 2D array in order to construct the array itself.
		/// </summary>
		/// <typeparam name="T">The type of array element.</typeparam>
		/// <param name="arraySpecifier">The string representation of a 2D array.</param>
		/// <param name="elementParser">The function used to parse individual elements.</param>
		/// <returns>The 2D array.</returns>
		public static T[,] ParseArray2D<T>(string arraySpecifier, Func<string,T> elementParser)
		{
			// Filter the array specifier to get rid of any whitespace, newlines, etc.
			arraySpecifier = new string(arraySpecifier.Where(c => !char.IsWhiteSpace(c)).ToArray());

			// Match a regular expression of the form "[width,height]listSpecifier".
			Regex regex = new Regex("\\[(?<width>[^,]+),(?<height>[^\\]]+)\\](?<listSpecifier>.+)");
			Match match = regex.Match(arraySpecifier);

			// Get the width, height and array elements from the match.
			int width = Convert.ToInt32(match.Groups["width"].ToString());
			int height = Convert.ToInt32(match.Groups["height"].ToString());
			List<T> arrayElements = ParseList(match.Groups["listSpecifier"].ToString(), elementParser);

			// Convert the 1D list into a 2D array with the right dimensions.
			var arr = new T[height,width];
			int index = 0;
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					arr[y,x] = arrayElements[index++];
				}
			}

			return arr;
		}

		/// <summary>
		/// Parses the string representation of a list in order to construct the list itself.
		/// </summary>
		/// <typeparam name="T">The type of list element.</typeparam>
		/// <param name="listSpecifier">The string representation of a list.</param>
		/// <param name="elementParser">The function used to parse individual elements.</param>
		/// <returns>The list.</returns>
		public static List<T> ParseList<T>(string listSpecifier, Func<string,T> elementParser)
		{
			return listSpecifier.Split(',').Select(s => elementParser(s.Trim())).ToList();
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
