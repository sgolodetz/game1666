/***
 * game1666proto3: EntityFootprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;

namespace game1666proto3
{
	/// <summary>
	/// Represents the grid-based 'footprint' of an entity on the terrain - i.e. the pattern
	/// of squares the entity will occupy on the grid.
	/// </summary>
	sealed class EntityFootprint
	{
		//#################### PROPERTIES ####################
		#region

		public Vector2i Hotspot	{ get; private set; }	/// the canonical grid square used to position the entity (the square in the pattern that will be under the user's mouse when placing the entity)
		public int[,] Pattern	{ get; private set; }	/// the pattern of grid squares that the entity will occupy

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity footprint.
		/// </summary>
		/// <param name="pattern">The pattern of grid squares that the entity will occupy.</param>
		/// <param name="hotspot">The canonical grid square used to position the entity.</param>
		/// <param name="orientation">The orientation of the entity.</param>
		public EntityFootprint(int[,] pattern, Vector2i hotspot, EntityOrientation orientation)
		{
			// TODO: Rotate the pattern as necessary based on the orientation.
			Pattern = pattern;
			Hotspot = hotspot;
		}

		#endregion
	}
}
