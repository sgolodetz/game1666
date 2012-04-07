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
	/// An instance of this class represents a component-based entity.
	/// </summary>
	sealed class Entity : IEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The children of the entity in its tree.
		/// </summary>
		private readonly IDictionary<string,IEntity> m_children = new Dictionary<string,IEntity>();

		/// <summary>
		/// The components of the entity.
		/// </summary>
		private readonly IDictionary<string,IEntityComponent> m_components = new Dictionary<string,IEntityComponent>();

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
		public IEntity Parent { get; set; }

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
			m_properties = new Dictionary<string,dynamic>
			{
				{ "Archetype", archetype },
				{ "Name", name }
			};
		}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public Entity(XElement entityElt)
		{
			m_properties = PropertyPersister.LoadProperties(entityElt);

			ObjectPersister.LoadChildObjects
			(
				entityElt,
				new ObjectLoader
				{
					CanBeUsedFor = t => t == typeof(Entity),
					AdditionalArguments = new object[] {},
					Load = o => AddChild(o)
				},
				new ObjectLoader
				{
					CanBeUsedFor = t => typeof(EntityComponent).IsAssignableFrom(t),
					AdditionalArguments = new object[] {},
					Load = o => (o as EntityComponent).AddToEntity(this)
				}
			);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child to add.</param>
		public void AddChild(IEntity child)
		{
			m_children.Add(child.Name, child);
			child.Parent = this;
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
		/// Gets the absolute path of this entity in its tree.
		/// </summary>
		/// <returns>The entity's absolute path.</returns>
		public string GetAbsolutePath()
		{
			IEntity cur = this;
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
		public IEntity GetChild(string name)
		{
			IEntity child = null;
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
		public IEntity GetEntityByAbsolutePath(string path)
		{
			return GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public IEntity GetEntityByAbsolutePath(LinkedList<string> path)
		{
			return GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public IEntity GetEntityByRelativePath(string path)
		{
			return GetEntityByRelativePath(new LinkedList<string>(path.Split('/')));
		}

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		public IEntity GetEntityByRelativePath(LinkedList<string> path)
		{
			IEntity cur = this;

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
		public void RemoveChild(IEntity child)
		{
			if(m_children.Remove(child.Name))
			{
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
			PropertyPersister.SaveProperties(entityElt, m_properties);
			ObjectPersister.SaveChildObjects(entityElt, m_components.Values);
			ObjectPersister.SaveChildObjects(entityElt, m_children.Values);
			return entityElt;
		}

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			foreach(IEntityComponent component in m_components.Values)
			{
				component.Update(gameTime);
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Gets the root entity of this entity's tree.
		/// </summary>
		/// <returns>The root entity of this entity's tree.</returns>
		private IEntity GetRootEntity()
		{
			IEntity cur = this;
			while(cur.Parent != null)
			{
				cur = cur.Parent;
			}
			return cur;
		}

		#endregion
	}
}
