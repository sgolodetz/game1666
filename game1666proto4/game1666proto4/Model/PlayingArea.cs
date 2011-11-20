/***
 * game1666proto4: PlayingArea.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	abstract class PlayingArea : CompositeModelEntity
	{
		//#################### PROPERTIES ####################
		#region

		public Terrain Terrain			{ get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public PlayingArea(Terrain terrain)
		{
			this.Terrain = terrain;
		}

		public PlayingArea(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public void AddEntity(Terrain terrain)
		{
			Terrain = terrain;
		}

		#endregion
	}
}
