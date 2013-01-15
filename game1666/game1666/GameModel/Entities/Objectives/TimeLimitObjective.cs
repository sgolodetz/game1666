/***
 * game1666: TimeLimitObjective.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameModel.Entities.Base;
using Microsoft.Xna.Framework;

namespace game1666.GameModel.Entities.Objectives
{
	/// <summary>
	/// TODO
	/// </summary>
	class TimeLimitObjective : Objective
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		private int TimeLimit { get { return Properties["TimeLimit"]; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="objectiveElt"></param>
		public TimeLimitObjective(XElement objectiveElt)
		:	base(objectiveElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines the current status of the objective in the specified world.
		/// </summary>
		/// <param name="world">The world in which to check the objective.</param>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		/// <returns>The current status of the objective in the specified world.</returns>
		public override ObjectiveStatus DetermineStatus(ModelEntity world, GameTime gameTime)
		{
			return gameTime.TotalGameTime.TotalMilliseconds < TimeLimit ? ObjectiveStatus.SATISFIED : ObjectiveStatus.FAILED;
		}

		/// <summary>
		/// TODO
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Time Limit (" + (TimeLimit / 1000.0) + " seconds)";
		}

		#endregion
	}
}
