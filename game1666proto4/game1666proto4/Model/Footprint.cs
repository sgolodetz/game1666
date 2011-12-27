/***
 * game1666proto4: Footprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents the 'footprint' of an entity.
	/// </summary>
	sealed class Footprint : Entity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The locations of the entrances to the entity, e.g. the doors into a building.
		/// </summary>
		private IList<Vector2i> m_entrances = new List<Vector2i>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The locations of the entrances to the entity, e.g. the doors into a building.
		/// </summary>
		public IEnumerable<Vector2i> Entrances { get { return m_entrances; } }

		/// <summary>
		/// The canonical grid square used to position the entity (the square in the
		/// pattern that will be under the user's mouse when placing the entity).
		/// </summary>
		public Vector2i Hotspot { get; private set; }

		/// <summary>
		/// The occupancy matrix specifying which squares are occupied by the entity.
		/// </summary>
		public bool[,] Occupancy { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a footprint from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the footprint's XML representation.</param>
		public Footprint(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		/// <summary>
		/// Constructs a footprint directly from its properties and a specified number of rotations.
		/// </summary>
		/// <param name="properties">The properties of the footprint.</param>
		/// <param name="rotations">The number of anti-clockwise 90 degree rotations to perform on the footprint.</param>
		private Footprint(IDictionary<string,dynamic> properties, int rotations)
		:	base(properties)
		{
			Initialise(rotations % 4);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines the average altitude of the footprint when placed at the specified
		/// position on the specified terrain.
		/// </summary>
		/// <param name="hotspotPosition">The position (on the terrain) to place the footprint's hotspot.</param>
		/// <param name="terrain">The terrain.</param>
		/// <returns>The average altitude of the footprint, if the footprint is within bounds, or null otherwise</returns>
		public float? DetermineAverageAltitude(Vector2i hotspotPosition, Terrain terrain)
		{
			IEnumerable<Vector2i> gridSquares = OverlaidGridSquares(hotspotPosition, terrain, true);
			if(gridSquares != null && gridSquares.Any())
			{
				return terrain.DetermineAverageAltitude(gridSquares);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Determines which grid squares in the specified terrain would be overlaid by placing the
		/// footprint's hotspot at the specified position.
		/// </summary>
		/// <param name="hotspotPosition">The position (on the terrain) to place the footprint's hotspot.</param>
		/// <param name="terrain">The terrain.</param>
		/// <param name="useOccupancy">Whether or not to take footprint occupancy into account.</param>
		/// <returns>A set of overlaid grid squares, if the footprint is within bounds, or null otherwise</returns>
		public IEnumerable<Vector2i> OverlaidGridSquares(Vector2i hotspotPosition, Terrain terrain, bool useOccupancy)
		{
			Vector2i offset = hotspotPosition - Hotspot;

			int footprintHeight = Occupancy.GetLength(0);
			int footprintWidth = Occupancy.GetLength(1);
			int gridHeight = terrain.Heightmap.GetLength(0) - 1;		// - 1 because grid height not heightmap height
			int gridWidth = terrain.Heightmap.GetLength(1) - 1;		// - 1 because grid width not heightmap width

			var gridSquares = new List<Vector2i>();
			for(int y = 0; y < footprintHeight; ++y)
			{
				int gridY = y + offset.Y;
				if(gridY < 0 || gridY >= gridHeight) return null;

				for(int x = 0; x < footprintWidth; ++x)
				{
					int gridX = x + offset.X;
					if(gridX < 0 || gridX >= gridWidth) return null;

					if(!useOccupancy || Occupancy[y,x])
					{
						gridSquares.Add(new Vector2i(gridX, gridY));
					}
				}
			}
			return gridSquares;
		}

		/// <summary>
		/// Constructs a clone of this footprint that is rotated by a specific number of anti-clockwise 90 degree turns.
		/// </summary>
		/// <param name="rotations">The number of anti-clockwise 90 degree rotations to perform on the footprint.</param>
		/// <returns>The clone.</returns>
		public Footprint Rotated(int rotations)
		{
			return new Footprint(new Dictionary<string,dynamic>(Properties), rotations);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the footprint from its properties and a specified number of rotations.
		/// </summary>
		/// <param name="rotations">The number of anti-clockwise 90 degree rotations to perform on the footprint.</param>
		private void Initialise(int rotations = 0)
		{
			// Perform the specified number of rotations on the footprint.
			for(int i = 0; i < rotations; ++i)
			{
				int patternHeight = Properties["Pattern"].GetLength(0);
				Properties["Hotspot"] = RotateHotspot(Properties["Hotspot"], patternHeight);
				Properties["Pattern"] = RotatePattern(Properties["Pattern"]);
			}

			Hotspot = Properties["Hotspot"];

			/*
			The footprint pattern specifies which squares are occupied by the entity and where the entrances are:
			- 0 means that the square is not occupied by the entity
			- 1 means that the square is occupied by the entity, but not an entrance
			- 2 means that the square is an entrance
			*/
			int[,] pattern = Properties["Pattern"];
			int height = pattern.GetLength(0);
			int width = pattern.GetLength(1);

			// Determine the occupancy matrix and the locations of the entrances to the entity from the pattern.
			Occupancy = new bool[height,width];
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					switch(pattern[y,x])
					{
						case 0:
							Occupancy[y,x] = false;
							break;
						case 1:
							Occupancy[y,x] = true;
							break;
						case 2:
							Occupancy[y,x] = true;
							m_entrances.Add(new Vector2i(x, y));
							break;
					}
				}
			}
		}

		/// <summary>
		/// Rotates the specified hotspot anti-clockwise by 90 degrees in a pattern with the specified height.
		/// </summary>
		/// <param name="hotspot">The hotspot.</param>
		/// <param name="patternHeight">The height of the pattern in which it is embedded.</param>
		/// <returns>The rotated hotspot.</returns>
		private static Vector2i RotateHotspot(Vector2i hotspot, int patternHeight)
		{
			return new Vector2i(patternHeight - 1 - hotspot.Y, hotspot.X);
		}

		/// <summary>
		/// Rotates a pattern anti-clockwise by 90 degrees.
		/// </summary>
		/// <param name="pattern">The pattern.</param>
		/// <returns>The rotated pattern.</returns>
		private static int[,] RotatePattern(int[,] pattern)
		{
			int height = pattern.GetLength(0);
			int width = pattern.GetLength(1);
			var rotatedPattern = new int[width, height];
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					rotatedPattern[x,height - 1 - y] = pattern[y,x];
				}
			}
			return rotatedPattern;
		}

		#endregion
	}
}
