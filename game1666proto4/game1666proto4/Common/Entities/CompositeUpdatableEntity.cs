/***
 * game1666proto4: CompositeUpdatableEntity.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a composite of updateable entities in the game.
	/// </summary>
	abstract class CompositeUpdatableEntity : CompositeEntity<IUpdateableEntity>, IUpdateableEntity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a composite updatable entity without any properties.
		/// </summary>
		public CompositeUpdatableEntity()
		{}

		/// <summary>
		/// Constructs a composite updatable entity directly from a set of properties.
		/// </summary>
		/// <param name="properties">The properties of the entity.</param>
		public CompositeUpdatableEntity(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a composite updatable entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the entity's XML representation.</param>
		public CompositeUpdatableEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the entity based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			foreach(IUpdateableEntity entity in Children)
			{
				entity.Update(gameTime);
			}
		}

		#endregion
	}
}
