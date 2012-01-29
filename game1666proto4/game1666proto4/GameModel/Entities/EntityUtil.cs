/***
 * game1666proto4: EntityUtil.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Placement;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// This class contains entity-related utility methods.
	/// </summary>
	static class EntityUtil
	{
		//#################### PUBLIC STATIC METHODS ####################
		#region

		/// <summary>
		/// Determines the model and orientation to use when drawing the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <param name="occupancyMap">The occupancy map associated with the terrain on which the entity sits.</param>
		/// <returns>The model and orientation to use.</returns>
		public static Tuple<string,Orientation4> DetermineModelNameAndOrientation(IPlaceableEntity entity, OccupancyMap occupancyMap)
		{
			return Tuple.Create(entity.Blueprint.Model, entity.Orientation);
		}

		/// <summary>
		/// Determines the model and orientation to use when drawing the specified road segment.
		/// </summary>
		/// <param name="roadSegment">The road segment.</param>
		/// /// <param name="occupancyMap">The occupancy map associated with the terrain on which the road segment sits.</param>
		/// <returns>The model and orientation to use.</returns>
		public static Tuple<string,Orientation4> DetermineModelNameAndOrientation(RoadSegment roadSegment, OccupancyMap occupancyMap)
		{
			int x = roadSegment.Position.X;
			int y = roadSegment.Position.Y;

			int which = 0;
			which += occupancyMap.LookupEntity(new Vector2i(x, y - 1)) != null ? 1 : 0;
			which += occupancyMap.LookupEntity(new Vector2i(x - 1, y)) != null ? 2 : 0;
			which += occupancyMap.LookupEntity(new Vector2i(x + 1, y)) != null ? 4 : 0;
			which += occupancyMap.LookupEntity(new Vector2i(x, y + 1)) != null ? 8 : 0;

			string suffix = "";
			Orientation4 orientation = roadSegment.Orientation;

			switch(which)
			{
				case 0:		suffix = "_Single";		orientation = Orientation4.XPOS; break;
				case 1:		suffix = "_End";		orientation = Orientation4.YNEG; break;
				case 2:		suffix = "_End";		orientation = Orientation4.XNEG; break;
				case 3:		suffix = "_Corner";		orientation = Orientation4.XNEG; break;
				case 4:		suffix = "_End";		orientation = Orientation4.XPOS; break;
				case 5:		suffix = "_Corner";		orientation = Orientation4.YNEG; break;
				case 6:		suffix = "_Straight";	orientation = Orientation4.XPOS; break;
				case 7:		suffix = "_TJunction";	orientation = Orientation4.YNEG; break;
				case 8:		suffix = "_End";		orientation = Orientation4.YPOS; break;
				case 9:		suffix = "_Straight";	orientation = Orientation4.YPOS; break;
				case 10:	suffix = "_Corner";		orientation = Orientation4.YPOS; break;
				case 11:	suffix = "_TJunction";	orientation = Orientation4.XNEG; break;
				case 12:	suffix = "_Corner";		orientation = Orientation4.XPOS; break;
				case 13:	suffix = "_TJunction";	orientation = Orientation4.XPOS; break;
				case 14:	suffix = "_TJunction";	orientation = Orientation4.YPOS; break;
				case 15:	suffix = "_Crossroads";	orientation = Orientation4.XPOS; break;
			}

			return Tuple.Create(roadSegment.Blueprint.Model + suffix, orientation);
		}

		#endregion
	}
}
