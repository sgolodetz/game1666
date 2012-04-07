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
				element = new XElement(elementName);
			}
			else
			{
				element = new XElement("object");
				element.Add(new XAttribute("type", type.FullName));
			}

			return element;
		}

		/// <summary>
		/// Creates objects from the non-property child elements of the specified XML element and
		/// performs arbitrary load actions on them (e.g. adding them to a parent object).
		/// </summary>
		/// <param name="parentElt">The parent XML element.</param>
		/// <param name="loaders">A set of loaders that specify how different types of object should be loaded.</param>
		public static void LoadChildObjects(XElement parentElt, params ObjectLoader[] loaders)
		{
			foreach(XElement childElt in parentElt.Elements().Where(e => e.Name != "property"))
			{
				// Determine the C# type corresponding to the child element.
				Type childType = DetermineElementType(childElt);

				// Try to find a suitable loader for that type.
				ObjectLoader loader = loaders.FirstOrDefault(L => L.CanBeUsedFor(childType));
				if(loader == null)
				{
					throw new InvalidOperationException("No matching loader for type: " + childType);
				}

				// Construct the array of arguments to pass to the child object's constructor.
				var arguments = new object[loader.AdditionalArguments.Length + 1];
				arguments[0] = childElt;
				loader.AdditionalArguments.CopyTo(arguments, 1);

				// Construct the child object and run the appropriate load action on it.
				loader.Load(Activator.CreateInstance(childType, arguments));
			}
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
				element.Add(child.SaveToXML());
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
				return s_specialElementsNameToType[elementName];
			}
			else if(elementName == "object")
			{
				string typename = Convert.ToString(element.Attribute("type").Value);
				var type = Type.GetType(typename);
				if(type != null) return type;
				else throw new InvalidDataException("No such type: " + typename);
			}
			else
			{
				throw new InvalidDataException("Cannot determine type of element with name: " + elementName);
			}
		}

		#endregion
	}
}
