/***
 * game1666: InternalComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of a class deriving from this one provides "internal" behaviour to a
	/// game model entity. For example, a world or settlement might have a playing area
	/// within it, or a house might need to manage its occupants. Internal components are
	/// intended to control what happens within an entity, as opposed to external components
	/// that control things like placement and movement.
	/// </summary>
	abstract class InternalComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blank internal component.
		/// </summary>
		protected InternalComponent()
		{}

		/// <summary>
		/// Constructs an internal component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected InternalComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion
	}
}
