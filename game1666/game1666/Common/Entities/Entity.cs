/***
 * game1666: Entity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of this class represents a component-based entity.
	/// Such entities consist of a set of pluggable components that
	/// define different aspects of their behaviour. In addition, they
	/// can be part of an "entity tree", which allows them to be looked
	/// up by path, e.g. "./settlement:Stuartopolis/building:House".
	/// </summary>
	sealed class Entity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The children of the entity in its tree.
		/// </summary>
		private readonly IDictionary<string,Entity> m_children = new Dictionary<string,Entity>();

		/// <summary>
		/// The components of the entity.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_components = new Dictionary<string,dynamic>();

		/// <summary>
		/// The properties of the entity.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The archetype of the entity. An entity's archetype, e.g. World,
		/// indicates which components the entity should have.
		/// </summary>
		public string Archetype { get { return m_properties["Archetype"]; } }

		/// <summary>
		/// The name of the entity (must be unique within its parent entity, if any).
		/// </summary>
		public string Name { get { return m_properties["Name"]; } }

		/// <summary>
		/// The parent of the entity in its tree.
		/// </summary>
		public Entity Parent { get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public Entity(IDictionary<string,dynamic> properties)
		{
			m_properties = properties;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child.</param>
		public void AddChild(Entity child)
		{
			m_children.Add(child.Name, child);
			child.Parent = this;
		}

		/// <summary>
		/// Gets the absolute path of this entity in its tree.
		/// </summary>
		/// <returns>The entity's absolute path.</returns>
		public string GetAbsolutePath()
		{
			Entity cur = this;
			var path = new LinkedList<string>();
			while(cur.Parent != null)
			{
				path.AddFirst(cur.Name);
				cur = cur.Parent;
			}
			path.AddFirst(".");
			return string.Join("/", path);
		}

		/// <summary>
		/// Looks up a child of this entity by name.
		/// </summary>
		/// <param name="name">The name of the child to look up.</param>
		/// <returns>The child with the specified name, if it exists, or null otherwise.</returns>
		public Entity GetChild(string name)
		{
			Entity child = null;
			m_children.TryGetValue(name, out child);
			return child;
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public Entity GetEntityByAbsolutePath(string path)
		{
			return GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public Entity GetEntityByAbsolutePath(LinkedList<string> path)
		{
			return GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public Entity GetEntityByRelativePath(string path)
		{
			return GetEntityByRelativePath(new LinkedList<string>(path.Split('/')));
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public Entity GetEntityByRelativePath(LinkedList<string> path)
		{
			Entity cur = this;

			while(cur != null && path.Count != 0)
			{
				switch(path.First())
				{
					case ".":
						break;
					case "..":
						cur = cur.Parent;
						break;
					default:
						cur = cur.GetChild(path.First());
						break;
				}

				path.RemoveFirst();
			}

			return cur;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Gets the root entity of this entity's tree.
		/// </summary>
		/// <returns>The root entity of this entity's tree.</returns>
		private Entity GetRootEntity()
		{
			Entity cur = this;
			while(cur.Parent != null)
			{
				cur = cur.Parent;
			}
			return cur;
		}

		#endregion
	}
}
