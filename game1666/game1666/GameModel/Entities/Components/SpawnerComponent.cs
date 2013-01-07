/***
 * game1666: SpawnerComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	/// An instance of this class allows its containing entity (known as a spawner) to spawn new mobile
	/// entities to help populate the world/city. Spawners are generally placed at the edge of the map.
	/// </summary>
	sealed class SpawnerComponent : ModelEntityComponent, IMatchmakingParticipant<ResourceOffer,ResourceRequest>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the component.
		/// </summary>
		public SpawnerBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Spawner"; } }

		/// <summary>
		/// The time remaining (in milliseconds) before another entity can be spawned.
		/// </summary>
		private int RemainingSpawnDelay
		{
			get { return Properties["RemainingSpawnDelay"]; }
			set { Properties["RemainingSpawnDelay"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a spawner component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		public SpawnerComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
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
			// No-op (nobody offers anything to a spawner component)
		}

		/// <summary>
		/// Informs the component of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void ConfirmMatchmakingRequest(ResourceRequest request, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
		{
			string prototypeName;
			if(Blueprint.Offers.TryGetValue(request.Resource.ToString(), out prototypeName))
			{
				// Construct a new entity based on the specified prototype.
				var spawnee = ModelEntity.CreateFromPrototype(prototypeName, null);

				// If the entity is being spawned in response to an occupancy request, set its home.
				// Its person component's state machine will cause it to head there in the absence
				// of anything better to do.
				if(request.Resource == Resource.OCCUPANCY)
				{
					var homeComponent = source as IHomeComponent;
					Debug.Assert(homeComponent != null);	// only home components should make occupancy requests

					var personComponent = spawnee.GetComponent<IPersonComponent>(ModelEntityComponentGroups.INTERNAL);
					personComponent.HomePath = homeComponent.Entity.GetAbsolutePath();
				}

				// Add the new entity to the world as a child of the spawner.
				Entity.AddChild(spawnee);

				// Set the remaining spawn delay to ensure that the spawner has to wait a bit before spawning anything else.
				RemainingSpawnDelay = Blueprint.SpawnDelay;
			}
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			RemainingSpawnDelay = Math.Max(RemainingSpawnDelay - gameTime.ElapsedGameTime.Milliseconds, 0);

			if(RemainingSpawnDelay == 0)
			{
				// Offer all the resources this spawner can provide.
				foreach(string resourceName in Blueprint.Offers.Keys)
				{
					this.Matchmaker().PostOffer
					(
						new ResourceOffer
						{
							Resource = (Resource)Enum.Parse(typeof(Resource), resourceName),
							AvailableQuantity = 1,
							AlreadyInGame = false
						},
						this
					);
				}
			}
		}

		#endregion
	}
}
