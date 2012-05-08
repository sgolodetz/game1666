/***
 * game1666: IEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component-based entity.
	/// Such entities consist of a set of pluggable components that define different aspects of
	/// their behaviour. In addition, they can be part of an "entity tree", which allows them to
	/// be looked up by path, e.g. "./settlement:Stuartopolis/house:Wibble". Note that all of
	/// the entities in such a tree must implement a common interface.
	/// </summary>
	/// <typeparam name="TreeEntityType">The type of entity used in the entity tree.</typeparam>
	interface IEntity<TreeEntityType> : IPersistableObject
		where TreeEntityType : class, IEntity<TreeEntityType>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The archetype of the entity. An entity's archetype, e.g. World,
		/// indicates which components the entity should have.
		/// </summary>
		string Archetype { get; }

		/// <summary>
		/// The children of the entity in its tree.
		/// </summary>
		IEnumerable<TreeEntityType> Children { get; }

		/// <summary>
		/// The name of the entity (must be unique within its parent entity, if any).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The parent of the entity in its tree.
		/// </summary>
		TreeEntityType Parent { get; set; }

		/// <summary>
		/// The properties of the entity.
		/// </summary>
		IDictionary<string,dynamic> Properties { get; }

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		TreeEntityType Self { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child to add.</param>
		void AddChild(TreeEntityType child);

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
		void AddComponentInternal(IEntityComponent component);

		/// <summary>
		/// Recursively loads the descendants of the entity from XML and adds them
		/// beneath the entity in the tree.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <returns>The entity.</returns>
		TreeEntityType AddDescendantsFromXML(XElement entityElt);

		/// <summary>
		/// Called just after this entity is added as the child of another.
		/// </summary>
		void AfterAdd();

		/// <summary>
		/// Called just before this entity is removed as the child of another.
		/// </summary>
		void BeforeRemove();

		/// <summary>
		/// Gets the absolute path of this entity in its tree.
		/// </summary>
		/// <returns>The entity's absolute path.</returns>
		string GetAbsolutePath();

		/// <summary>
		/// Looks up a child of this entity by name.
		/// </summary>
		/// <param name="name">The name of the child to look up.</param>
		/// <returns>The child with the specified name, if it exists, or null otherwise.</returns>
		TreeEntityType GetChild(string name);

		/// <summary>
		/// Looks up a component of this entity by group.
		/// </summary>
		/// <param name="group">The name of the component's group.</param>
		/// <returns>The component, if found, or null otherwise.</returns>
		dynamic GetComponent(string group);

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		TreeEntityType GetEntityByAbsolutePath(string path);

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		TreeEntityType GetEntityByRelativePath(string path);

		/// <summary>
		/// Gets the root entity of this entity's tree.
		/// </summary>
		/// <returns>The root entity of this entity's tree.</returns>
		TreeEntityType GetRootEntity();

		/// <summary>
		/// Removes a child from this entity, if present.
		/// </summary>
		/// <param name="child">The child to remove.</param>
		/// <exception cref="System.InvalidOperationException">If this entity does not contain the child.</exception>
		void RemoveChild(TreeEntityType child);

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
