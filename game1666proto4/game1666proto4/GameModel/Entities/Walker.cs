/***
 * game1666proto4: Walker.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a walker, e.g. a citizen walking around the city.
	/// </summary>
	sealed class Walker : ICompositeEntity, IMobileEntity, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The properties of the walker.
		/// </summary>
		private IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the walker.
		/// </summary>
		public MobileEntityBlueprint Blueprint { get; set; }

		/// <summary>
		/// The movement strategy for the entity.
		/// </summary>
		public IMovementStrategy MovementStrategy { private get; set; }

		/// <summary>
		/// The name of the walker (must be unique within its playing area).
		/// </summary>
		public string Name { get { return m_properties["Name"]; } }

		/// <summary>
		/// The 2D 45-degree orientation of the walker.
		/// </summary>
		public Orientation8 Orientation { get { return m_properties["Orientation"]; } }

		/// <summary>
		/// The position of the walker (relative to the origin of the containing entity).
		/// </summary>
		public Vector3 Position { get { return m_properties["Position"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a walker from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the walker's XML representation.</param>
		public Walker(XElement entityElt)
		{
			m_properties = EntityLoader.LoadProperties(entityElt);
			EntityLoader.LoadAndAddChildEntities(this, entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the walker based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity to add.</param>
		public void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Adds a movement strategy to the walker (note that there can only be one).
		/// </summary>
		/// <param name="movementStrategy">The movement strategy.</param>
		public void AddEntity(IMovementStrategy movementStrategy)
		{
			MovementStrategy = movementStrategy;
			MovementStrategy.EntityProperties = m_properties;
		}

		/// <summary>
		/// Updates the walker based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			if(MovementStrategy != null)
			{
				MovementStrategy.Move(gameTime);
			}
		}

		#endregion
	}
}
