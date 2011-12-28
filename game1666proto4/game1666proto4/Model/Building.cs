/***
 * game1666proto4: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.FSMs;
using game1666proto4.Common.Maths;
using Microsoft.Xna.Framework;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : CompositeEntity, IPlaceableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the building.
		/// </summary>
		public float Altitude { get { return Properties["Altitude"]; } }

		/// <summary>
		/// The blueprint for the building.
		/// </summary>
		public Blueprint Blueprint { get; private set; }

		/// <summary>
		/// The finite state machine for the building.
		/// </summary>
		public FiniteStateMachine<EntityStateID> FSM { get; private set; }

		/// <summary>
		/// The 2D axis-aligned orientation of the building.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a building directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the building.</param>
		/// <param name="initialStateID">The initial state of the building.</param>
		public Building(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		:	base(properties)
		{
			Initialise();

			// Construct and add the building's finite state machine.
			var fsmProperties = new Dictionary<string,dynamic>();
			fsmProperties["CurrentStateID"] = initialStateID.ToString();
			fsmProperties["TimeElapsed"] = 0;	// this is a new building, so no construction time has yet elapsed
			AddEntity(new EntityFSM(fsmProperties));
		}

		/// <summary>
		/// Constructs a building from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the building's XML representation.</param>
		public Building(XElement entityElt)
		:	base(entityElt)
		{
			Initialise();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds a finite state machine (FSM) to the building (note that there can only be one FSM).
		/// </summary>
		/// <param name="fsm">The FSM.</param>
		public void AddEntity(EntityFSM fsm)
		{
			FSM = fsm;
			fsm.EntityProperties = Properties;
		}

		/// <summary>
		/// Adds an entity to the building based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Makes a clone of this building that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public abstract IPlaceableEntity CloneNew();

		/// <summary>
		/// Checks whether or not the building can be validly placed on the specified terrain,
		/// bearing in mind its position and orientation.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>true, if it can be validly placed, or false otherwise</returns>
		public bool IsValidlyPlaced(Terrain terrain)
		{
			IEnumerable<Vector2i> gridSquares = Place(terrain);
			return gridSquares != null && gridSquares.Any() && !terrain.AreOccupied(gridSquares);
		}

		/// <summary>
		/// Attempts to place the building on the specified terrain.
		/// </summary>
		/// <param name="terrain">The terrain.</param>
		/// <returns>A set of grid squares that the building overlays, if it can be validly placed, or null otherwise</returns>
		public IEnumerable<Vector2i> Place(Terrain terrain)
		{
			Footprint footprint = Blueprint.Footprint.Rotated((int)Orientation);
			if(terrain.CalculateHeightRange(footprint.OverlaidGridSquares(Position, terrain, false)) == 0f)
			{
				return footprint.OverlaidGridSquares(Position, terrain, true);
			}
			else return null;
		}

		/// <summary>
		/// Updates the building based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Initialises the building from its properties.
		/// </summary>
		private void Initialise()
		{
			Blueprint = SceneGraph.GetEntityByPath("blueprints/" + Properties["Blueprint"]);
		}

		#endregion
	}
}
