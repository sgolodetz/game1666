﻿/***
 * game1666proto4: SceneGraph.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace game1666proto4
{
	/// <summary>
	/// This class represents the scene graph for the game.
	/// </summary>
	static class SceneGraph
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private static BlueprintManager s_blueprints;
		private static World s_world;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Loads the game's configuration data, e.g. entity blueprints.
		/// </summary>
		static SceneGraph()
		{
			LoadBlueprints();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Gets an entity in the scene graph by its (relative) path, e.g. "world/city:Stuartopolis".
		/// </summary>
		/// <param name="pathString">The path, as a string.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		public static dynamic GetEntityByPath(string pathString)
		{
			var path = new Queue<string>(pathString.Split('/').Where(s => !string.IsNullOrEmpty(s)));
			if(path.Count != 0)
			{
				string first = path.Dequeue();
				if(first == "blueprints")	return s_blueprints.GetEntityByPath(path);
				else if(first == "world")	return s_world.GetEntityByPath(path);
			}
			return null;
		}

		/// <summary>
		/// Loads a world from an XML file.
		/// </summary>
		/// <param name="worldFilename">The name of the XML file.</param>
		public static void LoadWorld(string worldFilename)
		{
			s_world = World.LoadFromFile(worldFilename);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Loads the entity blueprints.
		/// </summary>
		private static void LoadBlueprints()
		{
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			s_blueprints = new BlueprintManager(doc.XPathSelectElement("config/blueprints"));
		}

		#endregion
	}
}