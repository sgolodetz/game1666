/***
 * game1666: PersonComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using game1666.Common.Tasks;
using game1666.GameModel.Entities.Components.Extensions;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Components.Internal
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
	sealed class PersonComponent : InternalComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The person's internal priority queue of tasks.
		/// </summary>
		private readonly PriorityQueueTask m_queueTask = new PriorityQueueTask();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The person's home (as an absolute path in the entity tree).
		/// </summary>
		public string Home { get; set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Citizen"; } }

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
		public PersonComponent(XElement componentElt)
		:	base(componentElt)
		{}

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
				case PersonComponentState.ACTIVE:
				{
					// Try and execute the current task. If we run out of tasks, switch to the resting state.
					State = m_queueTask.Execute(gameTime) == TaskState.IN_PROGRESS ? PersonComponentState.ACTIVE : PersonComponentState.RESTING;
					break;
				}
				default:	// PersonComponentState.RESTING
				{
					// TODO: Assign the person a default task based on the time of day, and switch to the active state.
					m_queueTask.AddTask(this.TaskFactory().MakeGoToPositionTask(Entity, new Vector2(7.5f, 7.5f)), TaskPriority.LOW);
					State = PersonComponentState.ACTIVE;
					break;
				}
			}
		}

		#endregion
	}
}
