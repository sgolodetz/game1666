/***
 * game1666proto4: SceneGraph.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Communication;

namespace game1666proto4.GameModel
{
	/// <summary>
	/// This class represents the scene graph for the game.
	/// </summary>
	static class SceneGraph
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private static MessageSystem s_messageSystem = new MessageSystem();
		private static World s_world;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The scene graph's message system (for dispatching messages between game entities).
		/// </summary>
		public static MessageSystem MessageSystem { get { return s_messageSystem; } }

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
				switch(path.Dequeue())
				{
					case "world":
						return s_world.GetEntityByPath(path);
				}
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
	}
}
