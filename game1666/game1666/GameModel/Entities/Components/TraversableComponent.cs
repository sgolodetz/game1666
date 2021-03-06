﻿/***
 * game1666: TraversableComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Navigation;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// A traversable component is a placeable component that provides special rendering
	/// behaviour for road-like entities.
	/// </summary>
	sealed class TraversableComponent : PlaceableComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Traversable"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a traversable component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public TraversableComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines the actual model and orientation to use when drawing the traversable entity.
		/// For traversable entities such as road segments, we override the defaults in order to
		/// render joined-up roads in an aesthetically-pleasing way.
		/// </summary>
		/// <param name="modelName">The initial model name, as specified by the blueprint.</param>
		/// <param name="orientation">The initial orientation.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain on which the entity sits.</param>
		/// <returns>The actual model and orientation to use.</returns>
		public override Tuple<string,Orientation4> DetermineModelAndOrientation(string modelName, Orientation4 orientation, INavigationMap<ModelEntity> navigationMap)
		{
			string suffix = "";

			switch(ConstructEntranceBitfield(navigationMap))
			{
				case 0:		suffix = "_Single";		orientation = Orientation4.XPOS; break;
				case 1:		suffix = "_End";		orientation = Orientation4.YNEG; break;
				case 2:		suffix = "_End";		orientation = Orientation4.XNEG; break;
				case 3:		suffix = "_Corner";		orientation = Orientation4.XNEG; break;
				case 4:		suffix = "_End";		orientation = Orientation4.XPOS; break;
				case 5:		suffix = "_Corner";		orientation = Orientation4.YNEG; break;
				case 6:		suffix = "_Straight";	orientation = Orientation4.XPOS; break;
				case 7:		suffix = "_TJunction";	orientation = Orientation4.YNEG; break;
				case 8:		suffix = "_End";		orientation = Orientation4.YPOS; break;
				case 9:		suffix = "_Straight";	orientation = Orientation4.YPOS; break;
				case 10:	suffix = "_Corner";		orientation = Orientation4.YPOS; break;
				case 11:	suffix = "_TJunction";	orientation = Orientation4.XNEG; break;
				case 12:	suffix = "_Corner";		orientation = Orientation4.XPOS; break;
				case 13:	suffix = "_TJunction";	orientation = Orientation4.XPOS; break;
				case 14:	suffix = "_TJunction";	orientation = Orientation4.YPOS; break;
				case 15:	suffix = "_Crossroads";	orientation = Orientation4.XPOS; break;
			}

			return Tuple.Create(modelName + suffix, orientation);
		}

		#endregion

		//#################### PRIVATE STATIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a bitfield that compactly specifies the entrances surrounding the traversable entity.
		/// </summary>
		/// <param name="navigationMap">The navigation map associated with the terrain on which the entity sits.</param>
		/// <returns>The constructed bitfield.</returns>
		private int ConstructEntranceBitfield(INavigationMap<ModelEntity> navigationMap)
		{
			int x = Position.X;
			int y = Position.Y;

			int entranceBitfield = 0;
			entranceBitfield += IsEntityEntrance(new Vector2i(x, y - 1), navigationMap) ? 1 : 0;
			entranceBitfield += IsEntityEntrance(new Vector2i(x - 1, y), navigationMap) ? 2 : 0;
			entranceBitfield += IsEntityEntrance(new Vector2i(x + 1, y), navigationMap) ? 4 : 0;
			entranceBitfield += IsEntityEntrance(new Vector2i(x, y + 1), navigationMap) ? 8 : 0;
			return entranceBitfield;
		}

		/// <summary>
		/// Checks whether or not the specified grid square is occupied by an entrance to an entity.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain whose grid square is being checked.</param>
		/// <returns>true, if the specified grid square holds an entity entrance, or false otherwise.</returns>
		private static bool IsEntityEntrance(Vector2i gridSquare, INavigationMap<ModelEntity> navigationMap)
		{
			ModelEntity entity = navigationMap.LookupEntity(gridSquare);
			if(entity == null) return false;

			var placeableComponent = entity.GetComponent<IPlaceableComponent>(ModelEntityComponentGroups.EXTERNAL);
			return placeableComponent != null && placeableComponent.Entrances.Contains(gridSquare);
		}

		#endregion
	}
}
