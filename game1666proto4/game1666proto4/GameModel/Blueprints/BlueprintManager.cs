/***
 * game1666proto4: BlueprintManager.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class manages blueprints for the entities in the game.
	/// </summary>
	sealed class BlueprintManager : CompositeEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// Blueprints for the entities in the game.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_blueprints = new Dictionary<string,dynamic>();

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint manager from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the blueprint manager's XML representation.</param>
		public BlueprintManager(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a new blueprint to be managed.
		/// </summary>
		/// <param name="blueprint">The blueprint.</param>
		public override void AddEntityDynamic(dynamic blueprint)
		{
			m_blueprints[blueprint.Name] = blueprint;
		}

		/// <summary>
		/// Gets the blueprint with the specified name.
		/// </summary>
		/// <param name="name">The name of the blueprint to get.</param>
		/// <returns>The blueprint.</returns>
		public dynamic GetBlueprint(string name)
		{
			dynamic blueprint = null;
			m_blueprints.TryGetValue(name, out blueprint);
			return blueprint;
		}

		/// <summary>
		/// Gets a blueprint by its (relative) path, e.g. "Dwelling".
		/// </summary>
		/// <param name="path">The path to the blueprint.</param>
		/// <returns>The blueprint, if found, or null otherwise.</returns>
		public dynamic GetEntityByPath(Queue<string> path)
		{
			if(path.Count == 1) return GetBlueprint(path.Dequeue());
			else return null;
		}

		#endregion
	}
}
