/***
 * game1666proto4: Footprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents the 'footprint' of an entity on a terrain.
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
		/// The footprint pattern, specifying which squares are occupied by the entity and where the entrances are:
		/// * 0 means that the square is not occupied by the entity
		/// * 1 means that the square is occupied by the entity, but not an entrance
		/// * 2 means that the square is an entrance
		/// </summary>
		public int[,] Pattern { get; private set; }

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
			Hotspot = Properties["Hotspot"];
			Pattern = Properties["Pattern"];

			// Determine the locations of the entrances to the entity from the pattern.
			const int ENTRANCE_SPECIFIER = 2;
			int height = Pattern.GetLength(0);
			int width = Pattern.GetLength(1);
			for(int y = 0; y < height; ++y)
			{
				for(int x = 0; x < width; ++x)
				{
					if(Pattern[y,x] == ENTRANCE_SPECIFIER)
					{
						m_entrances.Add(new Vector2i(x, y));
					}
				}
			}
		}

		#endregion
	}
}
