/***
 * game1666: IObjective.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Objectives
{
	/// <summary>
	/// TODO
	/// </summary>
	enum ObjectiveStatus
	{
		FAILED,			// this objective cannot be satisfied
		SATISFIED,		// this objective is currently satisfied
		UNSATISFIED		// this objective is not currently satisfied
	}

	/// <summary>
	/// An instance of a class deriving from this one represents an objective
	/// that must be satisfied for a given world to be considered complete.
	/// </summary>
	abstract class Objective : IPersistableObject
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the objective.
		/// </summary>
		protected IDictionary<string,dynamic> Properties { get; private set; }

		/// <summary>
		/// TODO
		/// </summary>
		public virtual bool Required
		{
			get
			{
				dynamic required;
				return Properties.TryGetValue("Required", out required) ? required : false;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an objective from its XML representation.
		/// </summary>
		/// <param name="objectiveElt">The root element of the objective's XML representation.</param>
		protected Objective(XElement objectiveElt)
		{
			Properties = PropertyPersister.LoadProperties(objectiveElt);
		}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Determines the current status of the objective in the specified world.
		/// </summary>
		/// <param name="world">The world in which to check the objective.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The current status of the objective in the specified world.</returns>
		public abstract ObjectiveStatus DetermineStatus(ModelEntity world, GameTime gameTime);

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Saves the objective to XML.
		/// </summary>
		/// <returns>An XML representation of the objective.</returns>
		public XElement SaveToXML()
		{
			// TODO
			return null;
		}

		#endregion
	}
}
