/***
 * game1666proto4: EntityUtil.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.Common.Messages;
using game1666proto4.GameModel.Entities.Messages;

namespace game1666proto4.GameModel.Entities.Core
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
		public static Tuple<string,Orientation4> DetermineModelNameAndOrientation(IRoadSegment roadSegment, EntityNavigationMap navigationMap)
		{
			int x = roadSegment.Position.X;
			int y = roadSegment.Position.Y;

			int which = 0;
			which += IsEntityEntrance(new Vector2i(x, y - 1), navigationMap) ? 1 : 0;
			which += IsEntityEntrance(new Vector2i(x - 1, y), navigationMap) ? 2 : 0;
			which += IsEntityEntrance(new Vector2i(x + 1, y), navigationMap) ? 4 : 0;
			which += IsEntityEntrance(new Vector2i(x, y + 1), navigationMap) ? 8 : 0;

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
		/// </summary>
		/// <param name="entity">The entity being destructed.</param>
		/// <param name="playingArea">The playing area from which the entity is to be deleted.</param>
		public static void RegisterEntityDestructionRule(dynamic entity, IPlayingArea playingArea)
		{
			MessageSystem.RegisterRule
			(
				new MessageRule<EntityDestructionMessage>
				{
					Action = new Action<EntityDestructionMessage>(msg => playingArea.DeleteDynamicEntity(entity)),
					Entities = new List<dynamic> { entity, playingArea },
					Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(entity),
					Key = Guid.NewGuid().ToString()
				}
			);
		}

		/// <summary>
		/// Registers a message rule that responds to the spawning of an entity by adding it to a playing area.
		/// </summary>
		/// <param name="spawner">The entity doing the spawning.</param>
		/// <param name="playingArea">The playing area to which the entity is to be added.</param>
		public static void RegisterEntitySpawnRule(dynamic spawner, IPlayingArea playingArea)
		{
			// Note:	The explicit cast to ICompositeEntity in the code below really is necessary!
			//			There is a bug in the DLR that causes the dynamically-bound call to fail
			//			without the cast. See here for details:
			//			https://connect.microsoft.com/VisualStudio/feedback/details/597276/dynamic-runtime-fails-to-find-iset-t-contains-during-runtime
			MessageSystem.RegisterRule
			(
				new MessageRule<EntitySpawnMessage>
				{
					Action = new Action<EntitySpawnMessage>(msg => (playingArea as ICompositeEntity).AddDynamicEntity(msg.Entity)),
					Entities = new List<dynamic> { spawner, playingArea },
					Filter = MessageFilterFactory.TypedFromSource<EntitySpawnMessage>(spawner),
					Key = Guid.NewGuid().ToString()
				}
			);
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not the specified grid square is occupied by an entrance to an entity.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain whose grid square is being checked.</param>
		/// <returns>true, if the specified grid square holds an entity entrance, or false otherwise.</returns>
		private static bool IsEntityEntrance(Vector2i gridSquare, EntityNavigationMap navigationMap)
		{
			IPlaceableEntity entity = navigationMap.LookupEntity(gridSquare);
			return entity != null && entity.Entrances.Contains(gridSquare);
		}

		#endregion
	}
}
