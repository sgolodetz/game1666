/***
 * game1666proto4: RoadSegment.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Placement;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a road segment.
	/// </summary>
	sealed class RoadSegment : PlaceableEntity, IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The placement strategy for the road segment.
		/// </summary>
		public override IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a road segment directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the road segment.</param>
		/// <param name="initialStateID">The initial state of the road segment.</param>
		public RoadSegment(IDictionary<string,dynamic> properties, EntityStateID initialStateID)
		:	base(properties, initialStateID)
		{
			Properties["Name"] = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Constructs a road segment from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the road segment's XML representation.</param>
		public RoadSegment(XElement entityElt)
		:	base(entityElt)
		{
			Properties["Name"] = Guid.NewGuid().ToString();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this road segment that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new RoadSegment(Properties, EntityStateID.IN_CONSTRUCTION);
		}

		/// <summary>
		/// Updates the road segment based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
		}

		#endregion
	}
}
