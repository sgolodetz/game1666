/***
 * game1666: PlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Persistence;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components.Internal
{
	/// <summary>
	/// An instance of this class provides playing area behaviour to an entity such
	/// as the world or a settlement. Playing areas have a terrain on which other
	/// entities can be placed or move around.
	/// </summary>
	sealed class PlayingAreaComponent : BasicEntityComponent
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

		/// <summary>
		/// The playing area's terrain.
		/// </summary>
		public Terrain Terrain { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a playing area component with the specified terrain.
		/// </summary>
		/// <param name="terrain">The playing area's terrain.</param>
		public PlayingAreaComponent(Terrain terrain)
		{
			Terrain = terrain;
		}

		/// <summary>
		/// Constructs a playing area component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayingAreaComponent(XElement componentElt)
		:	base(componentElt)
		{
			ObjectPersister.LoadAndAddChildObjects
			(
				componentElt,
				new ChildObjectAdder
				{
					CanBeUsedFor = t => t == typeof(Terrain),
					AdditionalArguments = new object[] {},
					AddAction = o => Terrain = o
				}
			);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the component to XML.
		/// </summary>
		/// <returns>An XML representation of the component.</returns>
		public override XElement SaveToXML()
		{
			XElement componentElt = base.SaveToXML();
			ObjectPersister.SaveChildObjects(componentElt, new List<IPersistableObject> { Terrain });
			return componentElt;
		}

		#endregion
	}
}
