/***
 * game1666: ObjectPersister.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace game1666.Common.Persistence
{
	/// <summary>
	/// This class provides utility methods for saving/loading objects to/from XML.
	/// </summary>
	static class ObjectPersister
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A dictionary specifying how special XML element types such as "entity" get mapped to C# types.
		/// </summary>
		private static readonly IDictionary<string,Type> s_specialElementsNameToType = new Dictionary<string,Type>();

		/// <summary>
		/// A dictionary specifying how C# types get mapped to special XML element types such as "entity".
		/// </summary>
		private static readonly IDictionary<Type,string> s_specialElementsTypeToName = new Dictionary<Type,string>();

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a new XML element for an object of the specified type.
		/// </summary>
		/// <param name="type">The type of object for which to create an XML element.</param>
		/// <returns>The constructed element.</returns>
		public static XElement ConstructObjectElement(Type type)
		{
			XElement element;

			string elementName;
			if(s_specialElementsTypeToName.TryGetValue(type, out elementName))
			{
				// If this type of object has a special element type, use that.
				element = new XElement(elementName);
			}
			else
			{
				// Otherwise, create an "object" element and specify the object's type in full.
				element = new XElement("object");
				element.Add(new XAttribute("type", type.FullName));
			}

			return element;
		}

		/// <summary>
		/// Loads objects from all non-property child elements of the specified XML element
		/// that would yield objects of the specified type.
		/// </summary>
		/// <typeparam name="T">The type of child object to load.</typeparam>
		/// <param name="parentElt">The parent XML element.</param>
		/// <param name="additionalArguments">Any additional arguments to pass to the specified type's constructor.</param>
		/// <returns>The loaded child objects.</returns>
		public static IEnumerable<T> LoadChildObjects<T>(XElement parentElt, params object[] additionalArguments)
		{
			return LoadChildObjectsAndXML<T>(parentElt, additionalArguments).Select(t => t.Item1);
		}

		/// <summary>
		/// Loads objects from all non-property child elements of the specified XML element
		/// that would yield objects of the specified type, and returns them together with
		/// the corresponding XML elements from which they were created.
		/// </summary>
		/// <typeparam name="T">The type of child object to load.</typeparam>
		/// <param name="parentElt">The parent XML element.</param>
		/// <param name="additionalArguments">Any additional arguments to pass to the specified type's constructor.</param>
		/// <returns>The loaded child objects, together with the corresponding XML elements from which they were created.</returns>
		public static IEnumerable<Tuple<T,XElement>> LoadChildObjectsAndXML<T>(XElement parentElt, params object[] additionalArguments)
		{
			foreach(XElement childElt in parentElt.Elements().Where(e => e.Name != "property"))
			{
				// Determine the C# type corresponding to the child element.
				Type childType = DetermineElementType(childElt);

				// Load an object from the child element iff its type is convertible
				// to the type of object we want to load.
				if(typeof(T).IsAssignableFrom(childType))
				{
					yield return new Tuple<T,XElement>(LoadObject(childElt, additionalArguments), childElt);
				}
			}
		}

		/// <summary>
		/// Loads an object from an XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <param name="additionalArguments">Any additional arguments to pass to the object's constructor.</param>
		/// <returns>The constructed object.</returns>
		public static dynamic LoadObject(XElement element, params object[] additionalArguments)
		{
			// Determine the C# type corresponding to the element.
			Type type = DetermineElementType(element);

			// Construct the array of arguments to pass to the object's constructor.
			var arguments = new object[additionalArguments.Length + 1];
			arguments[0] = element;
			additionalArguments.CopyTo(arguments, 1);

			// Construct the object and return it.
			return Activator.CreateInstance(type, arguments);
		}

		/// <summary>
		/// Registers a special XML element type such as "entity". This will be used when
		/// determining the C# type corresponding to elements with the specified name.
		/// </summary>
		/// <remarks>
		/// The idea is to save users having to type out the full type name for objects in the XML.
		/// </remarks>
		/// <param name="elementName">The name of the XML element.</param>
		/// <param name="type">The C# type to which it corresponds.</param>
		public static void RegisterSpecialElement(string elementName, Type type)
		{
			s_specialElementsNameToType[elementName] = type;
			s_specialElementsTypeToName[type] = elementName;
		}

		/// <summary>
		/// Saves a set of child objects as children of an XML element.
		/// </summary>
		/// <param name="element">The XML element to which to save the child objects.</param>
		/// <param name="children">The child objects.</param>
		/// <returns>The XML element.</returns>
		public static XElement SaveChildObjects(XElement element, IEnumerable<IPersistableObject> children)
		{
			foreach(IPersistableObject child in children)
			{
				XElement childElt = child.SaveToXML();
				if(childElt != null)
				{
					element.Add(childElt);
				}
			}
			return element;
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Determines the C# type corresponding to the specified XML element.
		/// </summary>
		/// <param name="element">The XML element.</param>
		/// <returns>The corresponding C# type.</returns>
		private static Type DetermineElementType(XElement element)
		{
			string elementName = element.Name.ToString();

			if(s_specialElementsNameToType.ContainsKey(elementName))
			{
				// If this is one of the special element types, look up the corresponding C# type.
				return s_specialElementsNameToType[elementName];
			}
			else if(elementName == "object")
			{
				// If this is an "object" element, try and parse its type attribute to get the C# type.
				string typename = Convert.ToString(element.Attribute("type").Value);
				var type = Type.GetType(typename);
				if(type != null) return type;
				else throw new InvalidDataException("No such type: " + typename);
			}
			else
			{
				// Otherwise, a corresponding C# type cannot be determined for this element, so throw.
				throw new InvalidDataException("Cannot determine type of element with name: " + elementName);
			}
		}

		#endregion
	}
}
