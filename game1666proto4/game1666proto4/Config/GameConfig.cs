/***
 * game1666proto4: GameConfig.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace game1666proto4
{
	/// <summary>
	/// This class handles the loading and subsequent access of game configuration data such as entity blueprints.
	/// </summary>
	static class GameConfig
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private static IDictionary<string,dynamic> s_blueprints;	/// blueprints for all the entities in the game

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Gets the blueprint for entities with the specified name.
		/// </summary>
		/// <param name="entityName">The name of the entity for which to get the blueprint.</param>
		/// <returns>The blueprint.</returns>
		public static dynamic GetBlueprint(string entityName)
		{
			dynamic blueprint = null;
			s_blueprints.TryGetValue(entityName, blueprint);
			return blueprint;
		}

		/// <summary>
		/// Load game configuration data (e.g. entity blueprints) from the specified file.
		/// </summary>
		/// <param name="filename">The name of the file from which to load.</param>
		public static void Load(string filename)
		{
			// Note: I'm not doing any major validation here as this is purely a prototype.
			s_blueprints = new Dictionary<string,dynamic>();

			XDocument doc = XDocument.Load(filename);
			foreach(XElement blueprintNode in doc.XPathSelectElements("config/blueprints/blueprint"))
			{
				// Each blueprint can be used to create specific instances of an archetype - for example,
				// you might have a blueprint called "Dwelling" that creates a small instance of a house.
				// Our first job here is to get the entity name (Dwelling) and entity type (House).
				string entityName = Convert.ToString(blueprintNode.Attribute("name").Value);
				string entityTypename = Convert.ToString(blueprintNode.Attribute("type").Value);

				// Next, we look up the C# class corresponding to blueprints for this archetype - for the
				// example mentioned above, this would be HouseBlueprint.
				string blueprintTypename = "game1666proto4." + entityTypename + "Blueprint";
				Type blueprintType = Type.GetType(blueprintTypename);

				if(blueprintType != null)
				{
					// If such a class exists, we create the blueprint and store it for future use.
					s_blueprints[entityName] = Activator.CreateInstance(blueprintType, blueprintNode);
				}
				else
				{
					throw new System.InvalidOperationException("No such class: " + blueprintTypename);
				}
			}
		}

		#endregion
	}
}
