﻿/***
 * game1666: PlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class provides playing area behaviour to an entity such
	/// as the world or a settlement. Playing areas have a terrain on which other
	/// entities can be placed or move around.
	/// </summary>
	sealed class PlayingAreaComponent : ModelEntityComponent, IPlayingAreaComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entrances to the playing area.
		/// </summary>
		public List<Vector2i> Entrances { get { return Properties["Entrances"]; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayingArea"; } }

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		public INavigationMap<ModelEntity> NavigationMap { get; private set; }

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
			NavigationMap = new NavigationMap<ModelEntity,ModelEntityNavigationNode>(Terrain);
		}

		/// <summary>
		/// Constructs a playing area component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public PlayingAreaComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			Terrain = ObjectPersister.LoadChildObjects<Terrain>(componentElt).First();
			NavigationMap = new NavigationMap<ModelEntity,ModelEntityNavigationNode>(Terrain);
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
