/***
 * game1666: Entity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class deriving from this one represents a component-based entity.
	/// </summary>
	/// <typeparam name="TreeEntityType">The type of entity used in the entity tree.</typeparam>
	abstract class Entity<TreeEntityType> : IEntity<TreeEntityType>
		where TreeEntityType : class, IEntity<TreeEntityType>
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The children of the entity in its tree.
		/// </summary>
		private readonly IDictionary<string,TreeEntityType> m_children = new Dictionary<string,TreeEntityType>();

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
		public string Archetype { get { return Properties["Archetype"]; } }

		/// <summary>
		/// The children of the entity in its tree.
		/// </summary>
		public IEnumerable<TreeEntityType> Children { get { return m_children.Values; } }

		/// <summary>
		/// The name of the entity (must be unique within its parent entity, if any).
		/// </summary>
		public string Name { get { return Properties["Name"]; } }

		/// <summary>
		/// The parent of the entity in its tree.
		/// </summary>
		public TreeEntityType Parent { get; set; }

		/// <summary>
		/// The properties of the entity.
		/// </summary>
		public IDictionary<string,dynamic> Properties { get; private set; }

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public abstract TreeEntityType Self { get; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity directly from its name and archetype.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		protected Entity(string name, string archetype)
		{
			Properties = new Dictionary<string,dynamic>
			{
				{ "Archetype", archetype },
				{ "Name", name }
			};
		}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		protected Entity(XElement entityElt)
		{
			Properties = PropertyPersister.LoadProperties(entityElt);

			if(!Properties.ContainsKey("Name"))
			{
				Properties["Name"] = Guid.NewGuid().ToString();
			}

			foreach(var component in ObjectPersister.LoadChildObjects<EntityComponent<TreeEntityType>>(entityElt))
			{
				component.AddToEntity(Self);
			}
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child to add.</param>
		public void AddChild(TreeEntityType child)
		{
			m_children.Add(child.Name, child);
			child.Parent = Self;
			child.AfterAdd();
		}

		/// <summary>
		/// Adds a component to this entity.
		/// </summary>
		/// <remarks>
		/// Note: This method should not be called directly by clients, as it doesn't set
		/// the component's entity property (it can't without creating a dependency cycle
		/// between IEntity and IEntityComponent). Instead, clients who want to add a new
		/// component to an entity should call component.AddToEntity(entity).
		/// </remarks>
		/// <param name="component">The component to add.</param>
		/// <exception cref="System.InvalidOperationException">If the entity already has a component in the same group.</exception>
		public void AddComponentInternal(IEntityComponent component)
		{
			if(!m_components.ContainsKey(component.Group))
			{
				m_components.Add(component.Group, component);
			}
			else throw new InvalidOperationException("Group already has a component: " + component.Group);
		}

		/// <summary>
		/// Recursively loads the descendants of the entity from XML and adds them
		/// beneath the entity in the tree.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <returns>The entity.</returns>
		public TreeEntityType AddDescendantsFromXML(XElement entityElt)
		{
			foreach(var t in ObjectPersister.LoadChildObjectsAndXML<TreeEntityType>(entityElt))
			{
				TreeEntityType child = t.Item1;
				XElement childElt = t.Item2;

				AddChild(child);

				child.AddDescendantsFromXML(childElt);
			}
			return Self;
		}

		/// <summary>
		/// Called just after this entity is added as the child of another.
		/// </summary>
		public virtual void AfterAdd()
		{
			foreach(IEntityComponent component in m_components.Values)
			{
				component.AfterAdd();
			}
		}

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		public virtual void BeforeRemove()
		{
			foreach(IEntityComponent component in m_components.Values)
			{
				component.BeforeRemove();
			}
		}

		/// <summary>
		/// Gets the absolute path of this entity in its tree.
		/// </summary>
		/// <returns>The entity's absolute path.</returns>
		public string GetAbsolutePath()
		{
			TreeEntityType cur = Self;
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
		public TreeEntityType GetChild(string name)
		{
			TreeEntityType child = null;
			m_children.TryGetValue(name, out child);
			return child;
		}

		/// <summary>
		/// Looks up a component of this entity by group.
		/// </summary>
		/// <typeparam name="T">The type of the component.</typeparam>
		/// <param name="group">The name of the component's group.</param>
		/// <returns>The component, if found and of the correct type, or null otherwise.</returns>
		public T GetComponent<T>(string group) where T : class, IEntityComponent
		{
			IEntityComponent component = null;
			m_components.TryGetValue(group, out component);
			return component as T;
		}

		/// <summary>
		/// Checks whether or not the entity has a component with the specified group and name.
		/// </summary>
		/// <param name="group">The group that the component should have.</param>
		/// <param name="name">The name that the component should have.</param>
		/// <returns>true, if the entity has a component with the specified group and name, or false otherwise.</returns>
		public bool HasComponent(string group, string name)
		{
			IEntityComponent component = null;
			return m_components.TryGetValue(group, out component) && component.Name == name;
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public TreeEntityType GetEntityByAbsolutePath(string path)
		{
			return GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public TreeEntityType GetEntityByRelativePath(string path)
		{
			return GetEntityByRelativePath(new LinkedList<string>(path.Split('/')));
		}

		/// <summary>
		/// Gets the root entity of this entity's tree.
		/// </summary>
		/// <returns>The root entity of this entity's tree.</returns>
		public TreeEntityType GetRootEntity()
		{
			TreeEntityType cur = Self;
			while(cur.Parent != null)
			{
				cur = cur.Parent;
			}
			return cur;
		}

		/// <summary>
		/// Removes a child from this entity, if present.
		/// </summary>
		/// <param name="child">The child to remove.</param>
		/// <exception cref="System.InvalidOperationException">If this entity does not contain the child.</exception>
		public void RemoveChild(TreeEntityType child)
		{
			if(m_children.ContainsKey(child.Name))
			{
				child.BeforeRemove();
				m_children.Remove(child.Name);
				child.Parent = null;
			}
			else throw new InvalidOperationException("No such child: " + child.Name);
		}

		/// <summary>
		/// Saves the entity to XML.
		/// </summary>
		/// <returns>An XML representation of the entity.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = ObjectPersister.ConstructObjectElement(GetType());
			PropertyPersister.SaveProperties(entityElt, Properties);
			ObjectPersister.SaveChildObjects(entityElt, m_components.Values);
			ObjectPersister.SaveChildObjects(entityElt, m_children.Values);
			return entityElt;
		}

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			foreach(IEntityComponent component in m_components.Values)
			{
				component.Update(gameTime);
			}

			// Note: The .ToList() call here is deliberate - the list of children is allowed to change during the loop.
			foreach(TreeEntityType child in Children.ToList())
			{
				child.Update(gameTime);
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		private TreeEntityType GetEntityByRelativePath(LinkedList<string> path)
		{
			TreeEntityType cur = Self;

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
	}
}
