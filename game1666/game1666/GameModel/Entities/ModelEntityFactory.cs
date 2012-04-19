/***
 * game1666: ModelEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities
{
	/// <summary>
	/// This class constructs entities that form part of the game model.
	/// </summary>
	static class ModelEntityFactory
	{
		/// <summary>
		/// Makes a world entity with the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain of the world's playing area.</param>
		/// <returns>The constructed entity.</returns>
		public static IModelEntity MakeWorld(Terrain terrain)
		{
			var world = new ModelEntity(".", "World");
			new PlayingAreaComponent(terrain).AddToEntity(world);
			return world;
		}
	}
}
