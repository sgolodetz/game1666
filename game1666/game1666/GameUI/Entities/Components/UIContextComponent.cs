/***
 * game1666: UIContextComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using game1666.GameModel.Entities.Base;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Interfaces.Components;

namespace game1666.GameUI.Entities.Components
{
	/// <summary>
	/// An instance of this component provides UI context objects, such as the world being viewed,
	/// to a UI entity tree. It is intended for use as a component of the root entity of such a
	/// tree, e.g. a game view.
	/// </summary>
	sealed class UIContextComponent : UIEntityComponent, IUIContextComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return UIEntityComponentGroups.CONTEXT; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "UIContext"; } }

		/// <summary>
		/// The world that is being viewed by the entities in this UI entity tree.
		/// </summary>
		public ModelEntity World { get; private set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a UI context component.
		/// </summary>
		/// <param name="world">The world that is being viewed by the entities in this UI entity tree.</param>
		public UIContextComponent(ModelEntity world)
		{
			World = world;
		}

		#endregion
	}
}
