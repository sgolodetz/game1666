/***
 * game1666: PrototypeManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

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
		/// Creates a model entity based on the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype on which to base the entity.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		/// <returns>The entity, if the specified prototype exists, or null otherwise.</returns>
		public static ModelEntity CreateModelEntityFromPrototype(string prototypeName, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		{
			XElement prototypeElt = null;
			if(s_prototypes.TryGetValue(prototypeName, out prototypeElt))
			{
				var entity = new ModelEntity(prototypeElt, fixedProperties);
				entity.Prototype = prototypeName;
				return entity;
			}
			else return null;
		}

		/// <summary>
		/// Creates a UI entity based on the specified prototype.
		/// </summary>
		/// <param name="prototypeName">The name of the prototype on which to base the entity.</param>
		/// <param name="viewport">The viewport of the entity.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		/// <returns>The entity, if the specified prototype exists, or null otherwise.</returns>
		public static UIEntity CreateUIEntityFromPrototype(string prototypeName, Viewport viewport, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		{
			XElement prototypeElt = null;
			if(s_prototypes.TryGetValue(prototypeName, out prototypeElt))
			{
				var entity = new UIEntity(prototypeElt, fixedProperties);
				entity.Prototype = prototypeName;
				entity.Viewport = viewport;
				return entity;
			}
			else return null;
		}

		#endregion
	}
}
