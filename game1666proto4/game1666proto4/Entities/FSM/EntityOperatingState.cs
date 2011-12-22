/***
 * game1666proto4: EntityOperatingState.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// A state representing a time in which the entity is operational.
	/// </summary>
	sealed class EntityOperatingState : IFSMState<EntityStateID>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The ID of the state.
		/// </summary>
		public EntityStateID ID { get { return EntityStateID.OPERATING; } }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the state based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime) {}

		#endregion
	}
}
