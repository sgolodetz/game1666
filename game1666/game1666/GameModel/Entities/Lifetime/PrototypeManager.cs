/***
 * game1666: PrototypeManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Lifetime
{
	/// <summary>
	/// This class manages prototypes for entities in the game.
	/// </summary>
	static class PrototypeManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Prototypes for the entities in the game.
		/// </summary>
		private static readonly IDictionary<string,XElement> s_prototypes = new Dictionary<string,XElement>();

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Loads entity prototypes from the game configuration file.
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
					s_prototypes[prototypeName.Value] = entityElt;
				}
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Creates an entity based on the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype on which to base the entity.</param>
		/// <returns>The entity, if the specified prototype exists, or null otherwise.</returns>
		public static ModelEntity CreateEntityFromPrototype(string prototypeName)
		{
			XElement prototypeElt = null;
			if(s_prototypes.TryGetValue(prototypeName, out prototypeElt))
			{
				return new ModelEntity(prototypeElt);
			}
			else return null;
		}

		#endregion
	}
}
