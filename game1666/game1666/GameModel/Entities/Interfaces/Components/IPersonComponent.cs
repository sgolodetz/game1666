/***
 * game1666: IPersonComponent.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using game1666.Common.Entities;

namespace game1666.GameModel.Entities.Interfaces.Components
{
	/// <summary>
	/// An instance of a class implementing this interface provides person behaviour to its containing entity.
	/// </summary>
	interface IPersonComponent : IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The person's home (as an absolute path in the entity tree).
		/// </summary>
		string Home { get; set; }

		#endregion
	}
}
