/***
 * game1666: IBasicEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class implementing this interface represents a basic component-based entity.
	/// Such an entity doesn't have any additional properties beyond those provided for all entities.
	/// </summary>
	interface IBasicEntity : IEntity<IBasicEntity> {}
}
