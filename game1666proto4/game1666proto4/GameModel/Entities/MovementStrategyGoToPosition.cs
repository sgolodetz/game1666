/***
 * game1666proto4: MovementStrategyGoToPosition.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile entity to head towards a specific position.
	/// </summary>
	sealed class MovementStrategyGoToPosition : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The position towards which the entity should head.
		/// </summary>
		private Vector3 m_targetPosition;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new 'go to position' movement strategy.
		/// </summary>
		/// <param name="targetPosition">The position towards which the entity should head.</param>
		public MovementStrategyGoToPosition(Vector3 targetPosition)
		{
			m_targetPosition = targetPosition;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Moves the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Move(GameTime gameTime)
		{
			MobileEntityBlueprint blueprint = BlueprintManager.GetBlueprint(EntityProperties["Blueprint"]);

			Vector3 offset = m_targetPosition - EntityProperties["Position"];
			if(offset.Length() > Constants.EPSILON)
			{
				offset.Normalize();
				offset *= blueprint.MovementSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
				EntityProperties["Position"] += offset;
			}
		}

		#endregion
	}
}
