/***
 * game1666proto4: EntityUtil.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Messages;
using game1666proto4.GameModel.Messages;

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
		/// <param name="navigationMap">The navigation map associated with the terrain on which the entity sits.</param>
		/// <returns>The model and orientation to use.</returns>
		public static Tuple<string,Orientation4> DetermineModelNameAndOrientation(IPlaceableEntity entity, EntityNavigationMap navigationMap)
		{
			return Tuple.Create(entity.Blueprint.Model, entity.Orientation);
		}

		/// <summary>
		/// Determines the model and orientation to use when drawing the specified road segment.
		/// </summary>
		/// <param name="roadSegment">The road segment.</param>
		/// /// <param name="navigationMap">The navigation map associated with the terrain on which the road segment sits.</param>
		/// <returns>The model and orientation to use.</returns>
		public static Tuple<string,Orientation4> DetermineModelNameAndOrientation(RoadSegment roadSegment, EntityNavigationMap navigationMap)
		{
			int x = roadSegment.Position.X;
			int y = roadSegment.Position.Y;

			int which = 0;
			which += navigationMap.LookupEntity(new Vector2i(x, y - 1)) != null ? 1 : 0;
			which += navigationMap.LookupEntity(new Vector2i(x - 1, y)) != null ? 2 : 0;
			which += navigationMap.LookupEntity(new Vector2i(x + 1, y)) != null ? 4 : 0;
			which += navigationMap.LookupEntity(new Vector2i(x, y + 1)) != null ? 8 : 0;

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

		/// <summary>
		/// Registers a message rule that responds to the destruction of an entity by deleting it from a playing area.
		/// The message rule automatically unregisters itself once the entity has been deleted.
		/// </summary>
		/// <param name="entity">The entity in whose destruction we're interested.</param>
		/// <param name="playingArea">The playing area from which the entity is to be deleted.</param>
		public static void RegisterEntityDestructionRule(dynamic entity, IPlayingArea playingArea)
		{
			string key = entity.Name;

			MessageSystem.RegisterRule
			(
				MessageRuleFactory.FromSource
				(
					entity,
					new Action<EntityDestructionMessage>(msg =>
					{
						playingArea.DeleteDynamicEntity(entity);
						MessageSystem.UnregisterRule(key);
					}),
					key
				)
			);
		}

		#endregion
	}
}
