/***
 * game1666proto4: Spawner.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Matchmaking;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a spawner that can be used to generate new walkers
	/// to help populate the world/city. Spawners are generally placed at the edge of the map.
	/// </summary>
	sealed class Spawner : PlaceableEntity, IMatchmakingEntity<ResourceOffer,ResourceRequest>, IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The placement strategy for the spawner.
		/// </summary>
		public override IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a spawner directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the spawner.</param>
		/// <param name="initialStateID">The initial state of the spawner.</param>
		public Spawner(IDictionary<string,dynamic> properties, PlaceableEntityStateID initialStateID)
		:	base(properties, initialStateID)
		{
			SetName();
		}

		/// <summary>
		/// Constructs a spawner from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the house's XML representation.</param>
		public Spawner(XElement entityElt)
		:	base(entityElt)
		{
			SetName();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the spawner based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Makes a clone of this spawner that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new Spawner(Properties, PlaceableEntityStateID.IN_CONSTRUCTION);
		}

		/// <summary>
		/// Informs the spawner of a confirmed matchmaking offer.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		public void PostOffer(ResourceOffer offer, IMatchmakingEntity<ResourceOffer, ResourceRequest> source)
		{
			// No-op (nobody offers anything to a spawner)
		}

		/// <summary>
		/// Informs the spawner of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void PostRequest(ResourceRequest request, IMatchmakingEntity<ResourceOffer, ResourceRequest> source)
		{
			// TODO
			//System.Console.WriteLine(source + " is requesting " + request.DesiredQuantity + " of " + request.Resource);
		}

		/// <summary>
		/// Updates the spawner based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);

			// Offer to supply occupancy (by generating individual walkers to occupy houses).
			Matchmaker.PostOffer
			(
				new ResourceOffer
				{
					Resource = Resource.OCCUPANCY,
					AvailableQuantity = 1
				},
				this
			);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Makes sure that the spawner has an appropriate name.
		/// </summary>
		private void SetName()
		{
			Properties["Name"] = "spawner:" + Guid.NewGuid().ToString();
		}

		#endregion
	}
}
