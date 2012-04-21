/***
 * game1666: ExternalComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components.External
{
	/// <summary>
	/// An instance of a class deriving from this one provides "external" behaviour to a
	/// game model entity. For example, a house might be placeable on a terrain, or a
	/// person might move around on a terrain. External components are intended to control
	/// the entity's behaviour with respect to what is outside it, as opposed to internal
	/// components that control things like occupancy management.
	/// </summary>
	abstract class ExternalComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameModel/External"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an external component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected ExternalComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Draws the entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		public abstract void Draw(BasicEffect effect, float alpha);

		#endregion
	}
}
