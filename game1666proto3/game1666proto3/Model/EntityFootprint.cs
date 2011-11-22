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

		/// <summary>
		/// The canonical grid square used to position the entity (the square in the
		/// pattern that will be under the user's mouse when placing the entity).
		/// </summary>
		public Vector2i Hotspot	{ get; private set; }

		/// <summary>
		/// The pattern of grid squares that the entity will occupy.
		/// </summary>
		public int[,] Pattern	{ get; private set; }

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
			this.Pattern = pattern;
			this.Hotspot = hotspot;

			// Rotate the pattern and hotspot as necessary based on the orientation.
			int rotations = orientation - EntityOrientation.LEFT2RIGHT;
			for(int i = 0; i < rotations; ++i)
			{
				Rotate();
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Rotates the entity footprint anti-clockwise.
		/// </summary>
		private void Rotate()
		{
			int height = this.Pattern.GetLength(0), width = this.Pattern.GetLength(1);
			int[,] pattern = new int[width, height];
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					pattern[x,height - 1 - y] = this.Pattern[y,x];
				}
			}
			this.Pattern = pattern;

			this.Hotspot = new Vector2i(height - 1 - this.Hotspot.Y, this.Hotspot.X);
		}

		#endregion
	}
}
