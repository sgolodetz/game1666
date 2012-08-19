/***
 * game1666: BlueprintManager.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using game1666.Common.Persistence;

namespace game1666.GameModel.Entities.Blueprints
{
	/// <summary>
	/// This class manages blueprints for entity components in the game.
	/// </summary>
	static class BlueprintManager
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Blueprints for the entities in the game.
		/// </summary>
		private static readonly IDictionary<string,dynamic> s_blueprints = new Dictionary<string,dynamic>();

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Loads entity blueprints from the game configuration file.
		/// </summary>
		static BlueprintManager()
		{
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			foreach(dynamic blueprint in ObjectPersister.LoadChildObjects<dynamic>(doc.XPathSelectElement("config/blueprints")))
			{
				s_blueprints[blueprint.Name] = blueprint;
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Gets the blueprint with the specified name.
		/// </summary>
		/// <param name="name">The name of the blueprint to get.</param>
		/// <returns>The blueprint.</returns>
		public static dynamic GetBlueprint(string name)
		{
			dynamic blueprint = null;
			s_blueprints.TryGetValue(name, out blueprint);
			return blueprint;
		}

		#endregion
	}
}
