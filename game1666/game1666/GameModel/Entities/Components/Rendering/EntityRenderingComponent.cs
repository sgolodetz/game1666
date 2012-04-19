/***
 * game1666: EntityRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of a class deriving from this one provides rendering behaviour to a model entity.
	/// </summary>
	abstract class EntityRenderingComponent : ModelEntityComponent
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
		public static string StaticGroup { get { return "GameModel/Rendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected EntityRenderingComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Draws the model entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		public abstract void Draw(BasicEffect effect, float alpha);

		#endregion
	}
}
