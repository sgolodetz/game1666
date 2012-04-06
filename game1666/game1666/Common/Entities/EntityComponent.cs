/***
 * game1666: EntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class deriving from this one represents a component of an entity.
	/// </summary>
	abstract class EntityComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity containing this component (if any).
		/// </summary>
		public IEntity Entity { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public abstract string Group { get; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public abstract string Name { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds this component to an entity.
		/// </summary>
		/// <param name="entity">The entity to which to add the component.</param>
		public void AddToEntity(IEntity entity)
		{
			entity.AddComponentInternal(this);
			Entity = entity;
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			// No-op by default
		}

		#endregion
	}
}
