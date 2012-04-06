/***
 * game1666: IEntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a component of an entity.
	/// </summary>
	interface IEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		string Group { get; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		string Name { get; }

		#endregion
	}
}
