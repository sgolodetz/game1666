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
	/// An instance of this class specifies an object loader that can be used to perform arbitrary
	/// load actions on objects created by the ObjectPersister.LoadChildObjects method. The primary
	/// use for such a loader is to add the child object to a parent.
	/// </summary>
	sealed class ObjectLoader
	{
		/// <summary>
		/// Any additional arguments that need to be passed to the child object's constructor when it is created.
		/// </summary>
		public object[] AdditionalArguments { get; set; }

		/// <summary>
		/// A filter specifying the types for which this is the appropriate loader.
		/// </summary>
		public Func<Type,bool> CanBeUsedFor { get; set; }

		/// <summary>
		/// A custom load action to perform on the created child object.
		/// </summary>
		public Action<dynamic> Load { get; set; }
	}

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
		private static IDictionary<string,Type> s_specialElements = new Dictionary<string,Type>();

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

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
			s_specialElements.Add(elementName, type);
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

			if(s_specialElements.ContainsKey(elementName))
			{
				return s_specialElements[elementName];
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
