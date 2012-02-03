/***
 * game1666proto4: IMobileEntity.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents an entity that can move (e.g. a walker).
	/// </summary>
	interface IMobileEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		MobileEntityBlueprint Blueprint { get; set; }

		// TODO: MobileEntityFSM

		/// <summary>
		/// The name of the entity (must be unique within its playing area).
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The 2D 45-degree orientation of the entity.
		/// </summary>
		Orientation8 Orientation { get; }

		/// <summary>
		/// The position of the entity (relative to the origin of the containing entity).
		/// </summary>
		Vector3 Position { get; }

		#endregion
	}
}
