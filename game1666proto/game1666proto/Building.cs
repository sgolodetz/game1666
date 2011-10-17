/***
 * game1666proto: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto
{
	/// <summary>
	/// Represents a building in a city. For the purposes of the prototype, each building
	/// will just be a block placed in a particular position on the city plane.
	/// </summary>
	sealed class Building
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly Vector2 m_position;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building with the specified position on the city plane.
		/// </summary>
		/// <param name="position">The position of the building.</param>
		public Building(Vector2 position)
		{
			m_position = position;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the building.
		/// </summary>
		/// <param name="graphics">The graphics device.</param>
		/// <param name="basicEffect">The basic effect to use when drawing.</param>
		public void Draw(GraphicsDevice graphics, BasicEffect basicEffect)
		{
			// TODO
		}

		#endregion
	}
}
