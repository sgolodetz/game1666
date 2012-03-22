/***
 * game1666proto4: MovementStrategyGoToPosition.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile
	/// entity to head towards a specific position.
	/// </summary>
	sealed class MovementStrategyGoToPosition : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The properties of the movement strategy.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

		/// <summary>
		/// The sub-strategy used for path-following.
		/// </summary>
		/// <remarks>
		/// This movement strategy finds a path that will in principle take the
		/// entity to the desired position, then delegates the job of actually
		/// following the path to a 'follow path' strategy.
		/// </remarks>
		private MovementStrategyFollowPath m_subStrategy;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { private get; set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		public EntityNavigationMap NavigationMap { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' movement strategy from a target position.
		/// </summary>
		/// <param name="targetPosition">The target position.</param>
		public MovementStrategyGoToPosition(Vector2 targetPosition)
		{
			m_properties = new Dictionary<string,dynamic>();
			m_properties.Add("TargetPosition", targetPosition);
		}

		/// <summary>
		/// Constructs a 'go to position' movement strategy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the strategy's XML representation.</param>
		public MovementStrategyGoToPosition(XElement entityElt)
		{
			m_properties = EntityPersister.LoadProperties(entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Tries to move the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The result of the attempt: either blocked, finished or moved.</returns>
		public MoveResult Move(GameTime gameTime)
		{
			if(m_subStrategy == null)
			{
				// If we're not currently following a path to the target position, try and find one.
				Vector2 pos = EntityProperties["Position"];
				Queue<Vector2> path = NavigationMap.FindPath(pos, new List<Vector2> { m_properties["TargetPosition"] }, EntityProperties);

				// If we successfully found a path, set it as the one to follow; otherwise, exit.
				if(path != null)
				{
					m_subStrategy = new MovementStrategyFollowPath(path)
					{
						EntityProperties = this.EntityProperties,
						NavigationMap = this.NavigationMap
					};
				}
				else return MoveResult.BLOCKED;
			}

			// Attempt to follow the path that's been found.
			MoveResult result = m_subStrategy.Move(gameTime);

			// If the path found is blocked for some reason, clear the 'follow path' sub-strategy -
			// we'll try and find a new path next time.
			if(result == MoveResult.BLOCKED)
			{
				m_subStrategy = null;
			}

			return result;
		}

		/// <summary>
		/// Saves the movement strategy to XML.
		/// </summary>
		/// <returns>An XML representation of the movement strategy.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, m_properties);
			return entityElt;
		}

		#endregion
	}
}
