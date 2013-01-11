/***
 * game1666: TestObjective.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;

namespace game1666.GameModel.Entities.Objectives
{
	/// <summary>
	/// TODO
	/// </summary>
	class TestObjective : Objective
	{
		//#################### CONSTRUCTORS ####################
		#region

		// TODO

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Determines whether or not the objective is satisfied in the specified world.
		/// </summary>
		/// <param name="world">The world for which to check the objective.</param>
		/// <returns>true, if the objective is satisfied in the specified world, or false otherwise.</returns>
		public override bool IsSatisfied(ModelEntity world)
		{
			// TODO
			return false;
		}

		#endregion
	}
}
