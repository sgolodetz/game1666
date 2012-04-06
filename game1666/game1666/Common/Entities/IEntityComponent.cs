/***
 * game1666: IEntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component of an entity.
	/// </summary>
	interface IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		string Group { get; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		string Name { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		void Update(GameTime gameTime);

		#endregion
	}
}
