/***
 * game1666proto4: GameConfig.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace game1666proto4
{
	/// <summary>
	/// TODO
	/// </summary>
	static class GameConfig
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="filename"></param>
		public static void Load(string filename)
		{
			// Note: I'm not doing any major validation here as this is purely a prototype.
			XDocument doc = XDocument.Load(filename);
			foreach(XElement blueprintNode in doc.XPathSelectElements("config/blueprints/blueprint"))
			{
				string entityName = Convert.ToString(blueprintNode.Attribute("name").Value);
				string entityTypename = Convert.ToString(blueprintNode.Attribute("type").Value);
				string blueprintTypename = "game1666proto4." + entityTypename + "Blueprint";
				Type blueprintType = Type.GetType(blueprintTypename);
				var blueprint = Activator.CreateInstance(blueprintType, blueprintNode);
				// TODO
			}
		}

		#endregion
	}
}
