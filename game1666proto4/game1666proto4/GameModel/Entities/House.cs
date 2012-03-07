/***
 * game1666proto4: House.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Matchmaking;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a house.
	/// </summary>
	sealed class House : Building, IMatchmakingEntity<ResourceOffer,ResourceRequest>
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a house directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the house.</param>
		/// <param name="initialStateID">The initial state of the house.</param>
		public House(IDictionary<string,dynamic> properties, PlaceableEntityStateID initialStateID)
		:	base(properties, initialStateID)
		{}

		/// <summary>
		/// Constructs a house from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the house's XML representation.</param>
		public House(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Makes a clone of this house that is in the 'in construction' state.
		/// </summary>
		/// <returns>The clone.</returns>
		public override IPlaceableEntity CloneNew()
		{
			return new House(Properties, PlaceableEntityStateID.IN_CONSTRUCTION);
		}

		/// <summary>
		/// Informs the house of a confirmed matchmaking offer.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		public void PostOffer(ResourceOffer offer, IMatchmakingEntity<ResourceOffer, ResourceRequest> source)
		{
			// TODO
			//System.Console.WriteLine(source + " is offering to supply " + offer.AvailableQuantity + " of " + offer.Resource);
		}

		/// <summary>
		/// Informs the house of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void PostRequest(ResourceRequest request, IMatchmakingEntity<ResourceOffer, ResourceRequest> source)
		{
			// No-op (nobody requests anything from a house)
		}

		/// <summary>
		/// Updates the house based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			// If the house is not fully occupied, post an occupancy request to the matchmaker.
			if(true)	// TODO: Check current occupancy against maximum occupancy.
			{
				Matchmaker.PostRequest
				(
					new ResourceRequest
					{
						Resource = Resource.OCCUPANCY,
						DesiredQuantity = Blueprint.MaxOccupants,
						MinimumQuantity = 1
					},
					this
				);
			}
		}

		#endregion
	}
}
