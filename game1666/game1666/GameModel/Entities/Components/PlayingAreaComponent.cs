﻿/***
 * game1666: PlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.AbstractComponents;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Navigation;
using game1666.GameModel.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class provides playing area behaviour to an entity such
	/// as the world or a settlement. Playing areas have a terrain on which other
	/// entities can be placed or move around.
	/// </summary>
	sealed class PlayingAreaComponent : InternalComponent, IPlayingAreaComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayingArea"; } }

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		public INavigationMap<IModelEntity> NavigationMap { get; private set; }

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
			NavigationMap = new NavigationMap<IModelEntity,ModelEntityNavigationNode>(Terrain);
		}

		/// <summary>
		/// Constructs a playing area component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayingAreaComponent(XElement componentElt)
		:	base(componentElt)
		{
			Terrain = ObjectPersister.LoadChildObjects<Terrain>(componentElt).First();
			NavigationMap = new NavigationMap<IModelEntity,ModelEntityNavigationNode>(Terrain);
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
