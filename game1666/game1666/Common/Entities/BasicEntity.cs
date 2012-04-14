/***
 * game1666: BasicEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of this class represents a basic component-based entity. Such an entity
	/// doesn't have any additional properties beyond those provided for all entities.
	/// </summary>
	sealed class BasicEntity : Entity<IBasicEntity>, IBasicEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override IBasicEntity Self { get { return this; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an entity directly from its name and archetype.
		/// </summary>
		/// <param name="name">The name of the entity.</param>
		/// <param name="archetype">The archetype of the entity.</param>
		public BasicEntity(string name, string archetype)
		:	base(name, archetype)
		{}

		/// <summary>
		/// Constructs an entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		public BasicEntity(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
