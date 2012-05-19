/***
 * game1666: IExternalComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.Common.Entities;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides "external" behaviour to
	/// a game model entity. For example, a house might be placeable on a terrain, or a
	/// person might move around on a terrain. External components are intended to control
	/// the entity's behaviour with respect to what is outside it, as opposed to internal
	/// components that control things like occupancy management.
	/// </summary>
	interface IExternalComponent : IEntityComponent
	{
		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		/// <param name="parent">The parent of the entity (used when rendering entities that have not yet been attached to their parent).</param>
		void Draw(BasicEffect effect, float alpha, ModelEntity parent = null);

		#endregion
	}
}
