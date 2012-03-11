/***
 * game1666proto4: Spawner.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Matchmaking;
using game1666proto4.Common.Messages;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.FSMs;
using game1666proto4.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a spawner that can be used to generate new mobile entities
	/// to help populate the world/city. Spawners are generally placed at the edge of the map.
	/// </summary>
	sealed class Spawner : PlaceableEntity, IMatchmakingEntity<ResourceOffer,ResourceRequest>, IUpdateableEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The time remaining (in milliseconds) before another entity can be spawned.
		/// </summary>
		private int m_remainingSpawnDelay;

		#endregion

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
			// Spawn a new entity and make it head towards the entity making the request.
			string entityBlueprintName;
			if(Blueprint.Offers.TryGetValue(request.Resource.ToString(), out entityBlueprintName))
			{
				MobileEntityBlueprint entityBlueprint = BlueprintManager.GetBlueprint(entityBlueprintName);
				Type entityType = entityBlueprint.EntityType;

				// Set the properties of the entity.
				var entityProperties = new Dictionary<string,dynamic>();
				entityProperties["Blueprint"] = entityBlueprintName;
				entityProperties["Name"] = entityBlueprintName.ToLower() + ":" + Guid.NewGuid().ToString();
				entityProperties["Orientation"] = Properties["SpawnOrientation"];
				entityProperties["Position"] = Properties["SpawnPosition"];

				// Create the entity.
				IMobileEntity entity = Activator.CreateInstance(entityType, entityProperties) as IMobileEntity;

				// TODO: Set the proper movement strategy.
				//entity.MovementStrategy = new MovementStrategyGoToPosition(new Vector2(2.5f, 0.5f));
				//var pos = (source as IPlaceableEntity).Position;
				//entity.MovementStrategy = new MovementStrategyGoToPosition(new Vector2(pos.X + 1.5f, pos.Y + 0.5f));

				// Dispatch a spawn message so that the entity can be added to its playing area.
				MessageSystem.DispatchMessage(new EntitySpawnMessage(this, entity));

				// Add a rule to destruct the entity if its target is destroyed.
				MessageSystem.RegisterRule
				(
					new MessageRule<EntityDestructionMessage>
					{
						Action = msg => EntityLifetimeManager.QueueForDestruction(entity),
						Entities = new List<dynamic> { entity },
						Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(source),
						Key = Guid.NewGuid().ToString()
					}
				);

				// Set the remaining spawn delay to ensure that the spawner has to wait a bit before spawning anything else.
				m_remainingSpawnDelay = Blueprint.SpawnDelay;
			}
		}

		/// <summary>
		/// Updates the spawner based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
			m_remainingSpawnDelay = Math.Max(m_remainingSpawnDelay - gameTime.ElapsedGameTime.Milliseconds, 0);

			if(m_remainingSpawnDelay == 0)
			{
				// Offer all the resources this spawner can provide.
				foreach(string resourceName in Blueprint.Offers.Keys)
				{
					Matchmaker.PostOffer
					(
						new ResourceOffer
						{
							Resource = (Resource)Enum.Parse(typeof(Resource), resourceName),
							AvailableQuantity = 1
						},
						this
					);
				}
			}
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
