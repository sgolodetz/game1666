/***
 * game1666: EntityComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.Persistence;
using Microsoft.Xna.Framework;

namespace game1666.Common.Entities
{
	/// <summary>
	/// An instance of a class deriving from this one represents a component of an entity.
	/// </summary>
	/// <typeparam name="TreeEntityType">The type of entity used in the entity tree.</typeparam>
	abstract class EntityComponent<TreeEntityType> : IEntityComponent
		where TreeEntityType : class, IEntity<TreeEntityType>
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity containing the component (if any).
		/// </summary>
		internal TreeEntityType Entity { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public abstract string Group { get; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// The properties of the component.
		/// </summary>
		public IDictionary<string,dynamic> Properties { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blank component.
		/// </summary>
		protected EntityComponent()
		{
			Properties = new Dictionary<string,dynamic>();
		}

		/// <summary>
		/// Constructs a component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		protected EntityComponent(IDictionary<string,dynamic> properties)
		{
			Properties = properties;
		}

		/// <summary>
		/// Constructs a component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		protected EntityComponent(XElement componentElt)
		{
			Properties = PropertyPersister.LoadProperties(componentElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds this component to an entity.
		/// </summary>
		/// <param name="entity">The entity to which to add the component.</param>
		public void AddToEntity(TreeEntityType entity)
		{
			entity.AddComponentInternal(this);
			Entity = entity;
		}

		/// <summary>
		/// Called just after the entity containing this component is added as the child of another.
		/// </summary>
		public virtual void AfterAdd()
		{
			// No-op by default
		}

		/// <summary>
		/// Called just before the entity containing this component is removed as the child of another.
		/// </summary>
		public virtual void BeforeRemove()
		{
			// No-op by default
		}

		/// <summary>
		/// Saves the component to XML.
		/// </summary>
		/// <returns>An XML representation of the component.</returns>
		public virtual XElement SaveToXML()
		{
			XElement componentElt = ObjectPersister.ConstructObjectElement(GetType());
			PropertyPersister.SaveProperties(componentElt, Properties);
			return componentElt;
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public virtual void Update(GameTime gameTime)
		{
			// No-op by default
		}

		#endregion
	}
}
