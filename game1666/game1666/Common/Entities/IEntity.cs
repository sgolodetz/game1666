/***
 * game1666: IEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component-based entity.
	/// Such entities consist of a set of pluggable components that define different aspects of
	/// their behaviour. In addition, they can be part of an "entity tree", which allows them to
	/// be looked up by path, e.g. "./settlement:Stuartopolis/house:Wibble".
	/// </summary>
	interface IEntity : IPersistableObject
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
		IEnumerable<IEntity> Children { get; }

		/// <summary>
		/// The name of the entity (must be unique within its parent entity, if any).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The parent of the entity in its tree.
		/// </summary>
		IEntity Parent { get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a child to this entity.
		/// </summary>
		/// <param name="child">The child to add.</param>
		void AddChild(IEntity child);

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
		/// Casts this entity to a derived entity type.
		/// </summary>
		/// <typeparam name="T">The type of derived entity to which to cast.</typeparam>
		/// <returns>The casted entity.</returns>
		T As<T>() where T : class, IEntity;

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
		IEntity GetChild(string name);

		/// <summary>
		/// Looks up a component of this entity by group.
		/// </summary>
		/// <typeparam name="T">The type of the component (must be specified explicitly).</typeparam>
		/// <param name="group">The name of the component's group.</param>
		/// <returns>The component, if found, or null otherwise.</returns>
		T GetComponent<T>(string group) where T : class;

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		IEntity GetEntityByAbsolutePath(string path);

		/// <summary>
		/// Gets another entity in this entity's tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="path">The absolute path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		IEntity GetEntityByAbsolutePath(LinkedList<string> path);

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		IEntity GetEntityByRelativePath(string path);

		/// <summary>
		/// Gets another entity in this entity's tree by its path relative to this entity.
		/// </summary>
		/// <param name="path">The relative path to the other entity, as a list of path components.</param>
		/// <returns>The other entity, if found, or null otherwise.</returns>
		IEntity GetEntityByRelativePath(LinkedList<string> path);

		/// <summary>
		/// Initialises the entity once its entire tree has been constructed.
		/// </summary>
		/// <returns>The entity itself.</returns>
		IEntity Initialise();

		/// <summary>
		/// Removes a child from this entity, if present.
		/// </summary>
		/// <param name="child">The child to remove.</param>
		/// <exception cref="System.InvalidOperationException">If this entity does not contain the child.</exception>
		void RemoveChild(IEntity child);

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
