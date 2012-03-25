/***
 * game1666proto4: INamedEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;

namespace game1666proto4.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that is part of
	/// a 'name tree'. Given any entity in such a tree, it is possible to navigate to any other
	/// entity in the same tree via either a relative or absolute path string. The implementation
	/// of the path-based lookup functions is in the NamedEntity static class.
	/// </summary>
	interface INamedEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the entity (must be unique within its containing entity).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The parent of the entity (if any) in its name tree (or null if this is the root of the tree).
		/// </summary>
		INamedEntity Parent { get; set; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Gets a named entity directly contained within the current entity.
		/// </summary>
		/// <param name="name">The name of the entity to look up.</param>
		/// <returns>The entity, if found, or null otherwise.</returns>
		INamedEntity GetEntityByName(string name);

		#endregion
	}

	/// <summary>
	/// This class provides a set of path-based lookup functions for classes that implement INamedEntity.
	/// </summary>
	static class NamedEntity
	{
		//#################### PUBLIC EXTENSION METHODS ####################
		#region

		/// <summary>
		/// Gets the absolute path to the entity in its name tree.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>The absolute path to the entity.</returns>
		public static string GetAbsolutePath(this INamedEntity entity)
		{
			var path = new LinkedList<string>();
			while(entity.Parent != null)
			{
				path.AddFirst(entity.Name);
				entity = entity.Parent;
			}
			path.AddFirst(".");
			return string.Join("/", path);
		}

		/// <summary>
		/// Gets another entity in the name tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="entity">The starting entity.</param>
		/// <param name="path">The absolute path to the other entity.</param>
		/// <returns>The other entity.</returns>
		public static dynamic GetEntityByAbsolutePath(this INamedEntity entity, string path)
		{
			return entity.GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in the name tree by its absolute path (i.e. its path relative to the root entity).
		/// </summary>
		/// <param name="entity">The starting entity.</param>
		/// <param name="path">The absolute path to the other entity, as a list of path components.</param>
		/// <returns>The other entity.</returns>
		public static dynamic GetEntityByAbsolutePath(this INamedEntity entity, LinkedList<string> path)
		{
			return entity.GetRootEntity().GetEntityByRelativePath(path);
		}

		/// <summary>
		/// Gets another entity in the name tree by its path relative to the starting entity.
		/// </summary>
		/// <param name="entity">The starting entity.</param>
		/// <param name="path">The relative path to the other entity.</param>
		/// <returns>The other entity.</returns>
		public static dynamic GetEntityByRelativePath(this INamedEntity entity, string path)
		{
			return entity.GetEntityByRelativePath(new LinkedList<string>(path.Split('/')));
		}

		/// <summary>
		/// Gets another entity in the name tree by its path relative to the starting entity.
		/// </summary>
		/// <param name="entity">The starting entity.</param>
		/// <param name="path">The relative path to the other entity, as a list of path components.</param>
		/// <returns>The other entity.</returns>
		public static dynamic GetEntityByRelativePath(this INamedEntity entity, LinkedList<string> path)
		{
			while(path.Count != 0)
			{
				switch(path.First())
				{
					case ".":
						break;
					case "..":
						entity = entity.Parent;
						break;
					default:
						entity = entity.GetEntityByName(path.First());
						break;
				}

				path.RemoveFirst();
			}

			return entity;
		}

		#endregion

		//#################### PRIVATE EXTENSION METHODS ####################
		#region

		/// <summary>
		/// Gets the root of the specified entity's name tree.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>The root of the entity's name tree.</returns>
		private static INamedEntity GetRootEntity(this INamedEntity entity)
		{
			while(entity.Parent != null)
			{
				entity = entity.Parent;
			}
			return entity;
		}

		#endregion
	}
}
