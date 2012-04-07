/***
 * game1666: PlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;

namespace game1666.GameModel.Entities.Components.Internal
{
	/// <summary>
	/// An instance of this class provides playing area behaviour to an entity such
	/// as the world or a settlement. Playing areas have a terrain on which other
	/// entities can be placed or move around.
	/// </summary>
	sealed class PlayingAreaComponent : EntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayingArea"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameModel/Internal"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a playing area component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		public PlayingAreaComponent(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a playing area component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayingAreaComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion
	}
}
