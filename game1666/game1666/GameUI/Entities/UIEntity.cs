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
	/// An instance of this class represents a component-based entity that is part of the game's user interface.
	/// </summary>
	sealed class UIEntity : Entity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The viewport into which to draw the entity.
		/// </summary>
		public Viewport Viewport { get { return Properties["Viewport"]; } }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		public IEntity World { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI entity from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the entity's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public UIEntity(XElement entityElt, IEntity world)
		{
			Properties = PropertyPersister.LoadProperties(entityElt);
			World = world;

			ObjectPersister.LoadAndAddChildObjects
			(
				entityElt,
				new ChildObjectAdder
				{
					CanBeUsedFor = t => t == typeof(UIEntity),
					AdditionalArguments = new object[] { world },
					AddAction = o => AddChild(o)
				},
				new ChildObjectAdder
				{
					CanBeUsedFor = t => typeof(EntityComponent).IsAssignableFrom(t),
					AdditionalArguments = new object[] {},
					AddAction = o => (o as EntityComponent).AddToEntity(this)
				}
			);
		}

		#endregion
	}
}
