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
	/// This class handles the loading and subsequent access of game configuration data such as blueprints.
	/// </summary>
	static class GameConfig
	{
		//#################### CONSTANTS ####################
		#region

		public const float TERRAIN_SCALE_X = 4f;
		public const float TERRAIN_SCALE_Y = 4f;
		public const float TERRAIN_SCALE_Z = 1f;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		private static IDictionary<string,dynamic> s_blueprints;	/// blueprints for the entities in the game

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
			s_blueprints.TryGetValue(name, blueprint);
			return blueprint;
		}

		/// <summary>
		/// Load game configuration data (e.g. blueprints) from the specified file.
		/// </summary>
		/// <param name="filename">The name of the file from which to load.</param>
		public static void Load(string filename)
		{
			s_blueprints = new Dictionary<string,dynamic>();

			XDocument doc = XDocument.Load(filename);
			foreach(XElement blueprintElt in doc.XPathSelectElements("config/blueprints/entity"))
			{
				// Look up the C# type for the blueprint.
				string blueprintTypename = "game1666proto4." + Convert.ToString(blueprintElt.Attribute("type").Value);
				Type blueprintType = Type.GetType(blueprintTypename);

				if(blueprintType != null)
				{
					// Create the blueprint and store it for later use.
					dynamic blueprint = Activator.CreateInstance(blueprintType, blueprintElt);
					s_blueprints[blueprint.Name] = blueprint;
				}
				else
				{
					throw new InvalidOperationException("No such class: " + blueprintTypename);
				}
			}
		}

		#endregion
	}
}
