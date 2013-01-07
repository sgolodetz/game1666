/***
 * game1666: HomeComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Matchmaking;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Extensions;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class allows its containing entity to act as a home for entities.
	/// </summary>
	sealed class HomeComponent : ModelEntityComponent, IHomeComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the component.
		/// </summary>
		public HomeBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The number of people who currently live in this home.
		/// </summary>
		public int CurrentOccupants
		{
			get			{ return Properties["CurrentOccupants"]; }
			private set	{ Properties["CurrentOccupants"] = value; }
		}

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Home"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a home component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public HomeComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Informs the component of a confirmed matchmaking offer.
		/// </summary>
		/// <param name="offer">The offer.</param>
		/// <param name="source">The source of the offer.</param>
		public void ConfirmMatchmakingOffer(ResourceOffer offer, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
		{
			if(offer.Resource == Resource.OCCUPANCY)
			{
				CurrentOccupants = Math.Min(CurrentOccupants + offer.AvailableQuantity, Blueprint.MaxOccupants);
			}
		}

		/// <summary>
		/// Informs the component of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void ConfirmMatchmakingRequest(ResourceRequest request, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
		{
			// No-op (nobody requests anything from a home component)
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// If the home is not fully occupied, post an occupancy request to the matchmaker.
			if(CurrentOccupants < Blueprint.MaxOccupants)
			{
				this.Matchmaker().PostRequest
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
