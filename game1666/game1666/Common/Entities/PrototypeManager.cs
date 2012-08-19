/***
 * game1666: PrototypeManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace game1666.Common.Entities
{
	/// <summary>
	/// This class manages prototypes for entities in the game.
	/// </summary>
	static class PrototypeManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Prototype entity elements for the entities in the game.
		/// </summary>
		private static readonly IDictionary<string,XElement> s_prototypeEntityElts = new Dictionary<string,XElement>();

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Loads prototype entity elements from the game configuration file.
		/// </summary>
		static PrototypeManager()
		{
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			foreach(XElement prototypeElt in doc.XPathSelectElements("config/prototypes/prototype"))
			{
				XAttribute prototypeName = prototypeElt.Attribute("name");
				XElement entityElt = prototypeElt.Element("entity");
				if(prototypeName != null && entityElt != null)
				{
					s_prototypeEntityElts[prototypeName.Value] = entityElt;
				}
			}
		}

		#endregion

		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Gets the prototype entity element (if any) for the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype whose entity element we want to get.</param>
		/// <returns>The prototype entity element (if any), or null otherwise.</returns>
		public static XElement GetPrototypeEntityElement(string prototypeName)
		{
			XElement entityElt = null;
			s_prototypeEntityElts.TryGetValue(prototypeName, out entityElt);
			return entityElt;
		}

		#endregion
	}
}
