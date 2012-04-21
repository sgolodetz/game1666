/***
 * game1666: IEntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component of an entity.
	/// </summary>
	interface IEntityComponent : IPersistableObject
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		string Group { get; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		string Name { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after the component containing this entity is added as the child of another.
		/// </summary>
		void AfterAdd();

		/// <summary>
		/// Called just before the component containing this entity is removed as the child of another.
		/// </summary>
		void BeforeRemove();

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
