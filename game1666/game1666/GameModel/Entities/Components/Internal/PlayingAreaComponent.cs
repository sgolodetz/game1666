/***
 * game1666: PlayingAreaComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components.External;
using game1666.GameModel.Entities.Navigation;
using game1666.GameModel.Terrains;

namespace game1666.GameModel.Entities.Components.Internal
{
	/// <summary>
	/// An instance of this class provides playing area behaviour to an entity such
	/// as the world or a settlement. Playing areas have a terrain on which other
	/// entities can be placed or move around.
	/// </summary>
	sealed class PlayingAreaComponent : InternalComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		private readonly EntityNavigationMap m_navigationMap = new EntityNavigationMap();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The playing area's navigation map.
		/// </summary>
		public EntityNavigationMap NavigationMap { get { return m_navigationMap; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayingArea"; } }

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
			NavigationMap.Terrain = terrain;
		}

		/// <summary>
		/// Constructs a playing area component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayingAreaComponent(XElement componentElt)
		:	base(componentElt)
		{
			Terrain = ObjectPersister.LoadChildObjects<Terrain>(componentElt).First();
			NavigationMap.Terrain = Terrain;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Checks whether or not an entity can be validly placed on the terrain,
		/// bearing in mind its footprint, position and orientation.
		/// </summary>
		/// <param name="entity">The entity to be checked.</param>
		/// <returns>true, if the entity can be validly placed, or false otherwise.</returns>
		public bool IsValidlyPlaced(IModelEntity entity)
		{
			// Step 1:	Check that the entity occupies one or more grid squares, and that all the grid squares it does occupy are empty.
			PlaceableComponent placeableComponent = entity.GetComponent(PlaceableComponent.StaticGroup);
			IEnumerable<Vector2i> gridSquares = placeableComponent.Blueprint.PlacementStrategy.Place
			(
				Terrain,
				placeableComponent.Blueprint.Footprint,
				placeableComponent.Position,
				placeableComponent.Orientation
			);

			if(gridSquares == null || !gridSquares.Any() || NavigationMap.AreOccupied(gridSquares))
			{
				return false;
			}

			// Step 2:	Check that there are currently no mobile entities in the grid squares that the entity would occupy.
			//			Note that this isn't an especially efficient way of going about this, but it will do for now.
			//			A better approach would involve keeping track of which mobile entities are in which grid squares,
			//			and then checking per-grid square rather than per-entity.
			/*var gridSquareSet = new HashSet<Vector2i>(gridSquares);
			foreach(IMobileEntity mobile in Mobiles)
			{
				if(gridSquareSet.Contains(mobile.Position.ToVector2i()))
				{
					return false;
				}
			}*/

			// If we didn't find any problems, then the entity is validly placed.
			return true;
		}

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
