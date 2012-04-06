﻿/***
 * game1666: Entity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of this class represents a component-based entity.
	/// Such entities consist of a set of pluggable components that
	/// define different aspects of their behaviour. In addition, they
	/// can be part of an "entity tree", which allows them to be looked
	/// up by path, e.g. "./settlement:Stuartopolis/house:Wibble".
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
		private readonly IDictionary<string,IEntityComponent> m_components = new Dictionary<string,IEntityComponent>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The archetype of the entity. An entity's archetype, e.g. World,
		/// indicates which components the entity should have.
		/// </summary>
		public string Archetype { get; private set; }

		/// <summary>
		/// The name of the entity (must be unique within its parent entity, if any).
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The parent of the entity in its tree.
		/// </summary>
		public Entity Parent { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity directly from its name and archetype.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		public Entity(string name, string archetype)
		{
			Name = name;
			Archetype = archetype;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child to add.</param>
		public void AddChild(Entity child)
		{
			m_children.Add(child.Name, child);
			child.Parent = this;
		}

		/// <summary>
		/// Adds a component to this entity.
		/// </summary>
		/// <param name="component">The component to add.</param>
		/// <exception cref="System.InvalidOperationException">If the entity already has a component in the same group.</exception>
		public void AddComponent(IEntityComponent component)
		{
			if(!m_components.ContainsKey(component.Group))
			{
				m_components.Add(component.Group, component);
			}
			else throw new InvalidOperationException("Group already has a component: " + component.Group);
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
		/// Looks up a component of this entity by group.
		/// </summary>
		/// <typeparam name="T">The type of the component (must be specified explicitly).</typeparam>
		/// <param name="group">The name of the component's group.</param>
		/// <returns>The component, if found, or null otherwise.</returns>
		public T GetComponent<T>(string group) where T : class
		{
			IEntityComponent component = null;
			m_components.TryGetValue(group, out component);
			return component as T;
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

		/// <summary>
		/// Removes a child from this entity, if present.
		/// </summary>
		/// <param name="child">The child to remove.</param>
		/// <exception cref="System.InvalidOperationException">If this entity does not contain the child.</exception>
		public void RemoveChild(Entity child)
		{
			if(m_children.Remove(child.Name))
			{
				child.Parent = null;
			}
			else throw new InvalidOperationException("No such child: " + child.Name);
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
