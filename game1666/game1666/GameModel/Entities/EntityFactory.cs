/***
 * game1666: EntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using game1666.Common.Entities;
using game1666.GameModel.Entities.Components.Internal;

namespace game1666.GameModel.Entities
{
	/// <summary>
	/// This class constructs entities that form part of the game model.
	/// </summary>
	static class EntityFactory
	{
		/// <summary>
		/// Makes a new world entity.
		/// </summary>
		/// <returns>The constructed entity.</returns>
		public static Entity MakeWorld(/* TODO: Terrain */)
		{
			var world = new Entity(".", "World");

			new PlayingAreaComponent(new Dictionary<string,dynamic>
			{
			}).AddToEntity(world);

			return world;
		}
	}
}
