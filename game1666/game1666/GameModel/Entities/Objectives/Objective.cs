/***
 * game1666: IObjective.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Objectives
{
	/// <summary>
	/// An instance of a class deriving from this one represents an objective
	/// that must be satisfied for a given world to be considered complete.
	/// </summary>
	abstract class Objective : IPersistableObject
	{
		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Determines whether or not the objective is satisfied in the specified world.
		/// </summary>
		/// <param name="world">The world for which to check the objective.</param>
		/// <returns>true, if the objective is satisfied in the specified world, or false otherwise.</returns>
		public abstract bool IsSatisfied(ModelEntity world);

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
