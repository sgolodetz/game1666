/***
 * game1666proto4: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : Entity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		private readonly Vector2i m_position;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		public Vector2i Position { get { return m_position; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new building at the specified position.
		/// </summary>
		/// <param name="position">The position (relative to the origin of the containing entity) of the building's hotspot.</param>
		public Building(Vector2i position)
		{
			m_position = position;
		}

		#endregion
	}
}
