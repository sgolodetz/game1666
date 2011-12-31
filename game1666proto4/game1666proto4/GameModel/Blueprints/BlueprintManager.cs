/***
 * game1666proto4: BlueprintManager.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// This class manages blueprints for the entities in the game.
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
			foreach(dynamic blueprint in EntityLoader.LoadChildEntities(doc.XPathSelectElement("config/blueprints")))
			{
				AddBlueprint(blueprint);
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a new blueprint to be managed.
		/// </summary>
		/// <param name="blueprint">The blueprint.</param>
		public static void AddBlueprint(dynamic blueprint)
		{
			s_blueprints[blueprint.Name] = blueprint;
		}

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
