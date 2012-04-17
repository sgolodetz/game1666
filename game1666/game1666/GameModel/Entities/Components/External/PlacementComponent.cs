/***
 * game1666: PlacementComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.GameModel.Blueprints;

namespace game1666.GameModel.Entities.Components.External
{
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
	}
}
