/***
 * game1666: EntityPrototypeManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace game1666.Common.Entities
{
	/// <summary>
	/// This class manages prototypes for entities in the game. Prototype entities are stored in
	/// XML format, making it possible to reuse the code for loading previously-saved entities.
	/// </summary>
	static class EntityPrototypeManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Prototypes for the entities in the game, stored in XML format.
		/// </summary>
		private static readonly IDictionary<string,XElement> s_prototypeEntities = new Dictionary<string,XElement>();

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Loads prototype entities from the game configuration file.
		/// </summary>
		static EntityPrototypeManager()
		{
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			foreach(XElement prototypeElt in doc.XPathSelectElements("config/prototypes/prototype"))
			{
				XAttribute prototypeName = prototypeElt.Attribute("name");
				XElement entityElt = prototypeElt.Element("entity");
				if(prototypeName != null && entityElt != null)
				{
					s_prototypeEntities[prototypeName.Value] = entityElt;
				}
			}
		}

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the prototype entity (if any) for the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype entity we want to get.</param>
		/// <returns>The prototype entity (if any), or null otherwise.</returns>
		public static XElement GetPrototypeEntity(string prototypeName)
		{
			XElement entityElt = null;
			s_prototypeEntities.TryGetValue(prototypeName, out entityElt);
			return entityElt;
		}

		#endregion
	}
}
