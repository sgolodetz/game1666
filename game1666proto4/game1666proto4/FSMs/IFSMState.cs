/***
 * game1666proto4: IFSMState.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of a class implementing this type represents an individual state in a finite state machine.
	/// </summary>
	/// <typeparam name="StateID">The type of ID used to identify individual states.</typeparam>
	interface IFSMState<StateID>
	{
		/// <summary>
		/// The ID of the state.
		/// </summary>
		StateID ID { get; }

		/// <summary>
		/// Updates the state based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);
	}
}
