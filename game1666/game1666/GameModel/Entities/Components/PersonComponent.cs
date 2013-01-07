/***
 * game1666: PersonComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Matchmaking;
using game1666.Common.Persistence;
using game1666.Common.Tasks;
using game1666.Common.Tasks.RetryStrategies;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Extensions;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Matchmaking;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// The various possible states of a person component.
	/// </summary>
	enum PersonComponentState
	{
		ACTIVE,		// the entity containing the component is in the middle of performing a task
		RESTING		// the entity containing the component currently has no assigned tasks
	}

	/// <summary>
	/// An instance of this class provides person behaviour to its containing entity.
	/// </summary>
	sealed class PersonComponent : ModelEntityComponent, IPersonComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The person's internal priority queue of tasks.
		/// </summary>
		private readonly PriorityQueueTask m_queueTask;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		/// <summary>
		/// The person's home (if any).
		/// </summary>
		private ModelEntity Home
		{
			get { return HomePath != null ? Entity.GetEntityByAbsolutePath(HomePath) : null; }
		}

		/// <summary>
		/// The absolute path of the person's home (if any).
		/// </summary>
		public string HomePath { get; set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Person"; } }

		/// <summary>
		/// The state of the component.
		/// </summary>
		private PersonComponentState State
		{
			get	{ return (PersonComponentState)Enum.Parse(typeof(PersonComponentState), Properties["State"]); }
			set	{ Properties["State"] = value.ToString(); }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a person component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in.</param>
		public PersonComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			m_queueTask = ObjectPersister.LoadChildObjects<PriorityQueueTask>(componentElt).FirstOrDefault() ?? new PriorityQueueTask();
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
			// No-op (nobody offers anything to a person component)
		}

		/// <summary>
		/// Informs the component of a confirmed matchmaking request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="source">The source of the request.</param>
		public void ConfirmMatchmakingRequest(ResourceRequest request, IMatchmakingParticipant<ResourceOffer, ResourceRequest> source)
		{
			if(request.Resource == Resource.OCCUPANCY)
			{
				var homeComponent = source as IHomeComponent;
				Debug.Assert(homeComponent != null);	// only home components should make occupancy requests
				HomePath = homeComponent.Entity.GetAbsolutePath();
			}
		}

		/// <summary>
		/// Saves the component to XML.
		/// </summary>
		/// <returns>An XML representation of the component.</returns>
		public override XElement SaveToXML()
		{
			XElement componentElt = base.SaveToXML();
			ObjectPersister.SaveChildObjects(componentElt, new List<IPersistableObject> { m_queueTask });
			return componentElt;
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Try and ensure that the person has a home, etc.
			UpdateEssentials();

			// Execute any existing task the person has been assigned, or assign them a default
			// one if they don't currently have anything to do.
			switch(State)
			{
				case PersonComponentState.ACTIVE:
				{
					// Try and execute the current task. If we run out of tasks, switch to the resting state.
					State = m_queueTask.Execute(Entity, gameTime) == TaskState.IN_PROGRESS ? PersonComponentState.ACTIVE : PersonComponentState.RESTING;
					break;
				}
				default:	// PersonComponentState.RESTING
				{
					// TODO: Try and assign the person a default task based on the time of day.
					if(HomePath != null)
					{
						m_queueTask.AddTask(this.TaskFactory().MakeGoToEntityTask(HomePath, new AlwaysRetry()), TaskPriority.LOW);
						State = PersonComponentState.ACTIVE;
					}
					break;
				}
			}
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Try and make sure that the person has a home, etc.
		/// </summary>
		private void UpdateEssentials()
		{
			if(Home == null)
			{
				if(HomePath != null)
				{
					// The person's home has been destroyed, so clear the path to preserve consistency.
					HomePath = null;
				}

				// Offer occupancy to the matchmaker to try and find a new home.
				this.Matchmaker().PostOffer
				(
					new ResourceOffer
					{
						Resource = Resource.OCCUPANCY,
						AvailableQuantity = 1,
						AlreadyInGame = true
					},
					this
				);
			}
		}

		#endregion
	}
}
