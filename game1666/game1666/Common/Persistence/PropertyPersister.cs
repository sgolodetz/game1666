/***
 * game1666: PropertyPersister.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.Common.Persistence
{
	/// <summary>
	/// This class provides utility methods for saving/loading properties to/from XML.
	/// </summary>
	static class PropertyPersister
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

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
			parsers["Array2D[float]"] = s => ParseArray2D(s, Convert.ToSingle);
			parsers["Array2D[int]"] = s => ParseArray2D(s, Convert.ToInt32);
			parsers["bool"] = s => Convert.ToBoolean(s);
			parsers["Dictionary[string,string]"] = s => ParseDictionary(s, k => k, v => v);
			parsers["float"] = s => Convert.ToSingle(s);
			parsers["int"] = s => Convert.ToInt32(s);
			parsers["List[int]"] = s => ParseList(s, Convert.ToInt32);
			parsers["List[float]"] = s => ParseList(s, Convert.ToSingle);
			parsers["Orientation4"] = s => Enum.Parse(typeof(Orientation4), s);
			parsers["string"] = s => s;
			parsers["Vector2"] = s => ParseVector2Specifier(s);
			parsers["Vector2i"] = s => ParseVector2iSpecifier(s);
			parsers["Vector3"] = s => ParseVector3Specifier(s);
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
				string value = valueAttribute != null ? valueAttribute.Value : propertyElt.Value;

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
		/// <param name="separator">The separator used to delimit elements.</param>
		/// <returns>The 2D array.</returns>
		public static T[,] ParseArray2D<T>(string arraySpecifier, Func<string,T> elementParser, char separator = ',')
		{
			// Filter the array specifier to get rid of any whitespace, newlines, etc.
			arraySpecifier = new string(arraySpecifier.Where(c => !char.IsWhiteSpace(c)).ToArray());

			// Match a regular expression of the form "[width,height]listSpecifier".
			var regex = new Regex("\\[(?<width>[^,]+),(?<height>[^\\]]+)\\](?<listSpecifier>.*)");
			Match match = regex.Match(arraySpecifier);

			// Get the width, height and array elements from the match.
			int width = Convert.ToInt32(match.Groups["width"].ToString());
			int height = Convert.ToInt32(match.Groups["height"].ToString());
			List<T> arrayElements = ParseList(match.Groups["listSpecifier"].ToString(), elementParser, separator);

			// Check that the number of array elements matches the dimensions specified.
			if(arrayElements.Count != width * height)
			{
				throw new InvalidDataException("The number of elements in the 2D array does not match the dimensions specified.");
			}

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
		/// Parses the string representation of a dictionary in order to construct the dictionary itself.
		/// </summary>
		/// <typeparam name="K">The type of key used by the dictionary.</typeparam>
		/// <typeparam name="V">The type of value used by the dictionary.</typeparam>
		/// <param name="dictSpecifier">The string representation of a dictionary.</param>
		/// <param name="keyParser">The function used to parse individual keys.</param>
		/// <param name="valueParser">The function used to parse individual values.</param>
		/// <returns>The dictionary.</returns>
		public static Dictionary<K,V> ParseDictionary<K,V>(string dictSpecifier, Func<string,K> keyParser, Func<string,V> valueParser)
		{
			// Match a regular expression of the form "k1=v1,k2=v2,...,kn=vn".
			var regex = new Regex("([^=]+=[^,]+)(?:,([^=]+=[^,]+))*");
			Match match = regex.Match(dictSpecifier);

			// Construct the dictionary.
			var dict = new Dictionary<K,V>();

			// Note: We start at match group 1 because group 0 is for the whole expression.
			for(int i=1; i<match.Groups.Count; ++i)
			{
				string keyValueString = match.Groups[i].Value;
				if(string.IsNullOrWhiteSpace(keyValueString))
				{
					continue;
				}

				string[] keyValuePair = keyValueString.Split('=');
				if(keyValuePair.Length != 2)
				{
					throw new InvalidDataException("Bad key-value pair: " + keyValueString);
				}

				dict.Add(keyParser(keyValuePair[0].Trim()), valueParser(keyValuePair[1].Trim()));
			}

			return dict;
		}

		/// <summary>
		/// Parses the string representation of a list in order to construct the list itself.
		/// </summary>
		/// <typeparam name="T">The type of list element.</typeparam>
		/// <param name="listSpecifier">The string representation of a list.</param>
		/// <param name="elementParser">The function used to parse individual elements.</param>
		/// <param name="separator">The separator used to delimit elements.</param>
		/// <returns>The list.</returns>
		public static List<T> ParseList<T>(string listSpecifier, Func<string,T> elementParser, char separator = ',')
		{
			if(string.IsNullOrWhiteSpace(listSpecifier))
			{
				return new List<T>();
			}

			return listSpecifier.Split(separator).Select(s => elementParser(s.Trim())).ToList();
		}

		/// <summary>
		/// Parses the string representation of a Vector2 in order to construct the Vector2 itself.
		/// </summary>
		/// <param name="vectorSpecifier">The string representation of a Vector2.</param>
		/// <returns>The Vector2.</returns>
		public static Vector2 ParseVector2Specifier(string vectorSpecifier)
		{
			float[] values = vectorSpecifier
				.Split(',')
				.Where(v => !string.IsNullOrWhiteSpace(v))
				.Select(v => Convert.ToSingle(v.Trim()))
				.ToArray();

			if(values.Length == 2)
			{
				return new Vector2(values[0], values[1]);
			}
			else throw new InvalidDataException("The vector specifier '" + vectorSpecifier + "' does not have the right number of components.");
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
		/// Parses the string representation of a Vector3 in order to construct the Vector3 itself.
		/// </summary>
		/// <param name="vectorSpecifier">The string representation of a Vector3.</param>
		/// <returns>The Vector3.</returns>
		public static Vector3 ParseVector3Specifier(string vectorSpecifier)
		{
			float[] values = vectorSpecifier
				.Split(',')
				.Where(v => !string.IsNullOrWhiteSpace(v))
				.Select(v => Convert.ToSingle(v.Trim()))
				.ToArray();

			if(values.Length == 3)
			{
				return new Vector3(values[0], values[1], values[2]);
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

		/// <summary>
		/// Saves a 2D array to a string of the form "[width,height]elt00,elt01,...,eltmn".
		/// </summary>
		/// <typeparam name="T">The type of array element.</typeparam>
		/// <param name="arr">The array.</param>
		/// <returns>A string representation of the 2D array.</returns>
		public static string SaveArray2D<T>(T[,] arr)
		{
			int width = arr.GetLength(1);
			int height = arr.GetLength(0);

			var sb = new StringBuilder();
			sb.Append("[" + width + "," + height + "]");
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					sb.Append(arr[y,x]);
					if(!(y == height - 1 && x == width - 1)) sb.Append(",");
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Saves a set of properties as children of an XML element.
		/// </summary>
		/// <param name="entityElt">The XML element to which to save the properties.</param>
		/// <param name="properties">The properties to save.</param>
		/// <returns>The XML element.</returns>
		public static XElement SaveProperties(XElement entityElt, IEnumerable<KeyValuePair<string,dynamic>> properties)
		{
			// Set up the savers for the various supported types.
			var savers = new Dictionary<Type,Tuple<string,Func<dynamic,string>>>();
			var toStringSaver = new Func<dynamic,string>(v => v.ToString());
			savers[typeof(bool)] = Tuple.Create("bool", toStringSaver);
			savers[typeof(int)] = Tuple.Create("int", toStringSaver);
			savers[typeof(float)] = Tuple.Create("float", toStringSaver);
			savers[typeof(float[,])] = Tuple.Create("Array2D[float]", new Func<dynamic,string>(v => SaveArray2D<float>(v)));
			savers[typeof(Orientation4)] = Tuple.Create("Orientation4", toStringSaver);
			savers[typeof(string)] = Tuple.Create("string", new Func<dynamic,string>(v => v));
			savers[typeof(Vector2)] = Tuple.Create("Vector2", new Func<dynamic,string>(v => v.X + "," + v.Y));
			savers[typeof(Vector2i)] = Tuple.Create("Vector2i", toStringSaver);
			savers[typeof(Vector3)] = Tuple.Create("Vector3", new Func<dynamic,string>(v => v.X + "," + v.Y + "," + v.Z));

			foreach(var kv in properties.Where(p => p.Key != "Self"))
			{
				Tuple<string,Func<dynamic,string>> saver = savers[kv.Value.GetType()];

				var propertyElt = new XElement("property");
				propertyElt.Add(new XAttribute("name", kv.Key));
				propertyElt.Add(new XAttribute("type", saver.Item1));
				propertyElt.Add(new XAttribute("value", saver.Item2(kv.Value)));
				entityElt.Add(propertyElt);
			}

			return entityElt;
		}

		#endregion
	}
}
