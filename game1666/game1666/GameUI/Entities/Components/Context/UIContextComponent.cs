﻿/***
 * game1666: UIContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;

namespace game1666.GameUI.Entities.Components.Context
{
	/// <summary>
	/// An instance of this component provides UI context objects such as a UI entity factory
	/// to a UI entity tree. It is intended for use as a component of the root entity of such
	/// a tree, e.g. a game view.
	/// </summary>
	sealed class UIContextComponent : UIEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// A factory that can be used to construct UI entities.
		/// </summary>
		public IUIEntityFactory EntityFactory { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return StaticGroup; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "UIContext"; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public static string StaticGroup { get { return "GameUI/Context"; } }

		/// <summary>
		/// The world that is being viewed.
		/// </summary>
		public IModelEntity World { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI context component.
		/// </summary>
		/// <param name="world">The world that is being viewed.</param>
		/// <param name="entityFactory">A factory that can be used to construct UI entities.</param>
		public UIContextComponent(IModelEntity world, IUIEntityFactory entityFactory)
		{
			World = world;
			EntityFactory = entityFactory;
		}

		#endregion
	}
}
