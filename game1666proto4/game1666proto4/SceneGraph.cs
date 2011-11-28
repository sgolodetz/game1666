/***
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
	/// TODO
	/// </summary>
	static class SceneGraph
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private static BlueprintManager m_blueprints;
		private static World m_world;

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="pathString"></param>
		/// <returns></returns>
		public static dynamic GetEntityByPath(string pathString)
		{
			var path = new Queue<string>(pathString.Split('/').Where(s => !string.IsNullOrEmpty(s)));
			if(path.Count != 0)
			{
				string first = path.Dequeue();
				if(first == "world")
				{
					return m_world.GetEntityByPath(path);
				}
			}
			return null;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="worldFilename"></param>
		public static void Load(string worldFilename)
		{
			if(m_blueprints == null)
			{
				LoadBlueprints();
			}

			m_world = World.LoadFromFile(worldFilename);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		private static void LoadBlueprints()
		{
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			m_blueprints = new BlueprintManager(doc.XPathSelectElement("config/blueprints"));
		}

		#endregion
	}
}
