/***
 * game1666: UIEntityFactory.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Button;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Lifetime
{
	/// <summary>
	/// An instance of this class can be used to construct UI entities.
	/// </summary>
	sealed class UIEntityFactory : IUIEntityFactory
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// A lookup table of functions to make entities, keyed by archetype.
		/// </summary>
		private readonly static IDictionary<string,Func<Viewport,IDictionary<string,dynamic>,IUIEntity>> s_entityMakers;

		#endregion

		//#################### STATIC CONSTRUCTOR ####################
		#region

		/// <summary>
		/// Sets up the entity makers.
		/// </summary>
		static UIEntityFactory()
		{
			s_entityMakers = new Dictionary<string,Func<Viewport,IDictionary<string,dynamic>,IUIEntity>>();
			s_entityMakers.Add("Button", MakeButton);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Constructs a UI entity based on the specified archetype, viewport and properties.
		/// </summary>
		/// <param name="archetype">The archetype of the entity (e.g. Button).</param>
		/// <param name="viewport">The viewport of the entity.</param>
		/// <param name="properties">The properties of the various components of the entity.</param>
		/// <returns>The constructed entity.</returns>
		public IUIEntity MakeEntity(string archetype, Viewport viewport, IDictionary<string,dynamic> properties)
		{
			Func<Viewport,IDictionary<string,dynamic>,IUIEntity> entityMaker = null;
			if(s_entityMakers.TryGetValue(archetype, out entityMaker))
			{
				return entityMaker(viewport, properties);
			}
			else return null;
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Constructs a new button using the specified viewport and properties.
		/// </summary>
		/// <param name="viewport">The viewport of the button.</param>
		/// <param name="properties">The properties of the various components of the button.</param>
		/// <returns>The constructed button.</returns>
		private static IUIEntity MakeButton(Viewport viewport, IDictionary<string,dynamic> properties)
		{
			IUIEntity entity = new UIEntity(Guid.NewGuid().ToString(), "Button", viewport);
			new ButtonInteractionComponent().AddToEntity(entity);
			new ButtonRenderingComponent(properties["TextureName"]).AddToEntity(entity);
			return entity;
		}

		#endregion
	}
}
