/***
 * game1666: ObjectPersister.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Entities;

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
				Type childType = DetermineType(childElt);

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

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Determines the C# type corresponding to the specified XML element.
		/// </summary>
		/// <param name="elt">The XML element.</param>
		/// <returns>The corresponding C# type.</returns>
		private static Type DetermineType(XElement e)
		{
			switch(e.Name.ToString())
			{
				case "component":
					// TODO: Look up the appropriate type in the game configuration data.
					throw new NotImplementedException();
				case "entity":
					return typeof(Entity);
				case "object":
					string typename = "game1666." + Convert.ToString(e.Attribute("type").Value);
					var type = Type.GetType(typename);
					if(type != null) return type;
					else throw new InvalidDataException("No such class: " + typename);
				default:
					throw new InvalidDataException("Cannot load child element with name: " + e.Name);
			}
		}

		#endregion
	}
}
