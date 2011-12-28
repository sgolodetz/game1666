/***
 * game1666proto4: EntityOperatingState.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666proto4.Common.FSMs;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.FSMs
{
	/// <summary>
	/// A state representing a time in which the entity is operational.
	/// </summary>
	sealed class EntityOperatingState : IFSMState<EntityStateID>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// Supplies the properties of the entity whose state is managed by the containing FSM.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { set {} }

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
