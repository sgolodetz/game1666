/***
 * game1666proto4: City.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;

namespace game1666proto4
{
	sealed class City : PlayingArea
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		private readonly List<IBuilding> m_buildings = new List<IBuilding>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		public string Name { get { return Properties["Name"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		public City(string name, Terrain terrain)
		:	base(terrain)
		{
			Properties["Name"] = name;
		}

		public City(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
