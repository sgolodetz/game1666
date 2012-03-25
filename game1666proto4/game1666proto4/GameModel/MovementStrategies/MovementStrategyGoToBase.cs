/***
 * game1666proto4: MovementStrategyGoToBase.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.Core;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.MovementStrategies
{
	/// <summary>
	/// An instance of a class deriving from this one represents a movement strategy that
	/// causes a mobile entity to follow a sequence of one or more paths around the map,
	/// rerouting as necessary. The major purpose of this class is to take care of the
	/// rerouting logic so that derived classes don't need to worry about it.
	/// </summary>
	abstract class MovementStrategyGoToBase : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The sub-strategy used for path-following.
		/// </summary>
		/// <remarks>
		/// This movement strategy finds a path that will in principle take the
		/// entity to the desired place, then delegates the job of actually
		/// following the path to a 'follow path' strategy.
		/// </remarks>
		private MovementStrategyFollowPath m_subStrategy;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { protected get; set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		public EntityNavigationMap NavigationMap { protected get; set; }

		/// <summary>
		/// The properties of the movement strategy.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; set; }

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
			// If there's a valid 'follow path' sub-strategy in effect, use it, else try to generate a new one.
			m_subStrategy = m_subStrategy ?? GenerateSubStrategy();

			// If a sub-strategy couldn't be generated, we're blocked.
			if(m_subStrategy == null) return MoveResult.BLOCKED;

			// Otherwise, try and move the entity using the generated sub-strategy.
			MoveResult result = m_subStrategy.Move(gameTime);

			// If the sub-strategy couldn't make any progress, clear it - we'll try and find a new path next time.
			if(result == MoveResult.BLOCKED) m_subStrategy = null;

			return result;
		}

		/// <summary>
		/// Saves the movement strategy to XML.
		/// </summary>
		/// <returns>An XML representation of the movement strategy.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, Properties);
			return entityElt;
		}

		#endregion

		//#################### PROTECTED ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Tries to generate an appropriate 'follow path' sub-strategy for the mobile entity to follow.
		/// </summary>
		/// <returns>The generated sub-strategy, if any, or null otherwise.</returns>
		protected abstract MovementStrategyFollowPath GenerateSubStrategy();

		#endregion
	}
}
