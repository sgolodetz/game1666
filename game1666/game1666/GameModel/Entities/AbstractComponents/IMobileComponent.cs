/***
 * game1666: IMobileComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.AbstractComponents
{
	/// <summary>
	/// An instance of a class implementing this interface allows an entity to move around on a terrain.
	/// </summary>
	interface IMobileComponent : IExternalComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		float Altitude { get; set; }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		MobileBlueprint Blueprint { get; }

		/// <summary>
		/// The orientation of the entity (as an anti-clockwise angle in radians, where 0 means facing right).
		/// </summary>
		float Orientation { get; }

		/// <summary>
		/// The position of the entity (relative to the origin of the containing entity).
		/// </summary>
		Vector2 Position { get; }

		#endregion
	}
}
