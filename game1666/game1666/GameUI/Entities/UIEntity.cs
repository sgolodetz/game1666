/***
 * game1666: UIEntity.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities
{
	/// <summary>
	/// An instance of this class represents a component-based entity that is part of the game's UI.
	/// </summary>
	sealed class UIEntity : Entity<IUIEntity>, IUIEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity itself as a tree entity (this is necessary because we can't make IEntity implement TreeEntityType in C#).
		/// </summary>
		public override IUIEntity Self { get { return this; } }

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get { return Properties["Viewport"]; } }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		public IBasicEntity World { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public UIEntity(XElement entityElt, IBasicEntity world)
		{
			Properties = PropertyPersister.LoadProperties(entityElt);
			World = world;

			ObjectPersister.LoadAndAddChildObjects
			(
				entityElt,
				new ChildObjectAdder
				{
					CanBeUsedFor = t => typeof(IUIEntity).IsAssignableFrom(t),
					AdditionalArguments = new object[] { world },
					AddAction = o => AddChild(o)
				},
				new ChildObjectAdder
				{
					CanBeUsedFor = t => typeof(EntityComponent<IUIEntity>).IsAssignableFrom(t),
					AdditionalArguments = new object[] {},
					AddAction = o => (o as EntityComponent<IUIEntity>).AddToEntity(this)
				}
			);
		}

		#endregion
	}
}
