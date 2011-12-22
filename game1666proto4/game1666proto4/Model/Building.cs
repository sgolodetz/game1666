/***
 * game1666proto4: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;
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
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a building from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the building's XML representation.</param>
		public Building(XElement entityElt)
		:	base(entityElt)
		{
			Blueprint = SceneGraph.GetEntityByPath("blueprints/" + Properties["Blueprint"]);
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
		/// Updates the building based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
		}

		#endregion
	}
}
