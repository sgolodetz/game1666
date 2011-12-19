/***
 * game1666proto4: EntityFSM.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class is used to manage the building of an entity over time.
	/// </summary>
	sealed class EntityFSM
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The overall time required to build the entity (in milliseconds).
		/// </summary>
		private int m_timeToBuild;

		/// <summary>
		/// The remaining time required to build the entity (in milliseconds).
		/// </summary>
		private int m_timeToCompletion;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity builder that will take the specified amount of time to finish building the entity.
		/// </summary>
		/// <param name="timeToBuild">The overall time required to build the entity (in milliseconds).</param>
		public EntityFSM(int timeToBuild)
		{
			m_timeToBuild = m_timeToCompletion = timeToBuild;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			// TODO
		}

		#endregion
	}
}
