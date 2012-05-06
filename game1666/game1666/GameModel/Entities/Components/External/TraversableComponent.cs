/***
 * game1666: TraversableComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Navigation;

namespace game1666.GameModel.Entities.Components.External
{
	/// <summary>
	/// A traversable component is a placeable component that provides special rendering
	/// behaviour for road-like entities.
	/// </summary>
	sealed class TraversableComponent : PlaceableComponent
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a traversable component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		public TraversableComponent(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a traversable component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public TraversableComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Determines the actual model and orientation to use when drawing the traversable entity.
		/// For traversable entities such as road-segments, we override the defaults in order to
		/// render joined-up roads in an aesthetically-pleasing way.
		/// </summary>
		/// <param name="modelName">The initial model name, as specified by the blueprint.</param>
		/// <param name="orientation">The initial orientation.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain on which the entity sits.</param>
		/// <returns>The actual model and orientation to use.</returns>
		protected override Tuple<string,Orientation4> DetermineModelAndOrientation(string modelName, Orientation4 orientation, EntityNavigationMap navigationMap)
		{
			int x = Position.X;
			int y = Position.Y;

			int which = 0;
			which += IsEntityEntrance(new Vector2i(x, y - 1), navigationMap) ? 1 : 0;
			which += IsEntityEntrance(new Vector2i(x - 1, y), navigationMap) ? 2 : 0;
			which += IsEntityEntrance(new Vector2i(x + 1, y), navigationMap) ? 4 : 0;
			which += IsEntityEntrance(new Vector2i(x, y + 1), navigationMap) ? 8 : 0;

			string suffix = "";

			switch(which)
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
		/// Checks whether or not the specified grid square is occupied by an entrance to an entity.
		/// </summary>
		/// <param name="gridSquare">The grid square.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain whose grid square is being checked.</param>
		/// <returns>true, if the specified grid square holds an entity entrance, or false otherwise.</returns>
		private static bool IsEntityEntrance(Vector2i gridSquare, EntityNavigationMap navigationMap)
		{
			IModelEntity entity = navigationMap.LookupEntity(gridSquare);
			if(entity == null) return false;

			PlaceableComponent placeableComponent = entity.GetComponent(PlaceableComponent.StaticGroup);
			return placeableComponent != null && placeableComponent.Entrances.Contains(gridSquare);
		}

		#endregion
	}
}
