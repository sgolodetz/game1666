﻿/***
 * game1666: PlacementComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.GameModel.Blueprints;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components.External
{
	/// <summary>
	/// The various possible states of a placement component.
	/// </summary>
	enum PlacementComponentState
	{
		IN_CONSTRUCTION,	// the entity containing the component is in the process of being constructed
		IN_DESTRUCTION,		// the entity containing the component is in the process of being destructed
		OPERATING			// the entity containing the component is operating normally
	}

	/// <summary>
	/// An instance of this class provides placement behaviour to an entity,
	/// i.e. entities with this component can be placed on a terrain.
	/// </summary>
	sealed class PlacementComponent : ExternalComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The footprint of the entity, suitably rotated to take account of its orientation.
		/// </summary>
		private readonly Footprint m_footprint;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		public float Altitude
		{
			get { return Properties["Altitude"]; }
			set { Properties["Altitude"] = value; }
		}

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		public PlacementBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The amount of construction done (in comparison to the time required to construct the entity).
		/// </summary>
		private int ConstructionDone
		{
			get { return Properties["ConstructionDone"]; }
			set { Properties["ConstructionDone"] = Math.Max(Math.Min(value, Blueprint.TimeToConstruct), 0); }
		}

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		public bool Destructible
		{
			get
			{
				dynamic destructible;
				return Properties.TryGetValue("Destructible", out destructible) ? destructible : true;
			}
		}

		/// <summary>
		/// The entrances to the entity.
		/// </summary>
		public IEnumerable<Vector2i> Entrances
		{
			get
			{
				Vector2i offset = Position - m_footprint.Hotspot;
				return m_footprint.Entrances.Select(e => e + offset);
			}
		}

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Placement"; } }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		/// <summary>
		/// The state of the component.
		/// </summary>
		public PlacementComponentState State
		{
			get { return Enum.Parse(typeof(PlacementComponentState), Properties["State"]); }
			set { Properties["State"] = value.ToString(); }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placement component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlacementComponent(XElement componentElt)
		:	base(componentElt)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);

			// Determine the entity's (suitably rotated) footprint.
			m_footprint = Blueprint.Footprint.Rotated((int)Orientation);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			switch(State)
			{
				case PlacementComponentState.IN_CONSTRUCTION:
				{
					ConstructionDone += gameTime.ElapsedGameTime.Milliseconds;
					if(ConstructionDone == Blueprint.TimeToConstruct)
					{
						State = PlacementComponentState.OPERATING;
					}
					break;
				}
				case PlacementComponentState.IN_DESTRUCTION:
				{
					ConstructionDone -= gameTime.ElapsedGameTime.Milliseconds;
					if(ConstructionDone == 0)
					{
						// TODO
						//EntityDestructionManager.QueueForDestruction(Entity);
					}
					break;
				}
				default:	// PlacementComponentState.OPERATING
				{
					break;
				}
			}
		}

		#endregion
	}
}
