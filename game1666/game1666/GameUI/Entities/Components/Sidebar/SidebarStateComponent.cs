/***
 * game1666: SidebarStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameUI.Entities.Components.Button;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components.Sidebar
{
	/// <summary>
	/// An instance of this class manages the state for a sidebar viewer.
	/// </summary>
	sealed class SidebarStateComponent : StateComponent
	{
		//#################### CONSTANTS ####################
		#region

		private const bool ENSURE_SQUARE_BUTTONS = true;

		private const int MAX_BUTTON_WIDTH = int.MaxValue;
		private const int MAX_BUTTON_HEIGHT = int.MaxValue;

		private const int HORIZONTAL_SPACING = 20;
		private const int VERTICAL_SPACING = 20;

		#endregion

		//#################### NESTED TYPES ####################
		#region

		/// <summary>
		/// An instance of this class represents the way in which a set of buttons should be laid out (in a grid).
		/// </summary>
		private sealed class ButtonLayout
		{
			/// <summary>
			/// The height of each button.
			/// </summary>
			public int ButtonHeight { get; set; }

			/// <summary>
			/// The width of each button.
			/// </summary>
			public int ButtonWidth { get; set; }

			/// <summary>
			/// The number of columns in the button grid.
			/// </summary>
			public int Columns { get; set; }

			/// <summary>
			/// The width of each column in the button grid (including the horizontal spacing between columns).
			/// </summary>
			public int ColumnWidth { get; set; }

			/// <summary>
			/// The height of each row in the button grid (including the vertical spacing between rows).
			/// </summary>
			public int RowHeight { get; set; }

			/// <summary>
			/// The number of rows in the button grid.
			/// </summary>
			public int Rows { get; set; }

			/// <summary>
			/// The horizontal offset of the top-left button from the top-left of the viewport for the button set.
			/// </summary>
			public int XOffset { get; set; }
		}

		/// <summary>
		/// An instance of this class specifies all the information necessary to construct a button.
		/// </summary>
		private sealed class ButtonSpecifier
		{
			/// <summary>
			/// A hook that the button can use to check whether or not it should be drawn highlighted.
			/// </summary>
			public Func<bool> IsHighlighted { get; set; }

			/// <summary>
			/// Invoked when a mouse button is pressed.
			/// </summary>
			public MouseEvent MousePressedHook { get; set; }

			/// <summary>
			/// The name of the texture to use when drawing the button.
			/// </summary>
			public string TextureName { get; set; }
		}

		/// <summary>
		/// An instance of this class specifies all the information necessary to describe what an element button should do.
		/// </summary>
		private sealed class ElementSpecifier
		{
			/// <summary>
			/// The name to pass to the tool (e.g. the entity blueprint name for tools that place entities).
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// The name of the tool this button will use (e.g. "EntityPlacementTool").
			/// </summary>
			public string Tool { get; set; }
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The currently-selected sidebar group.
		/// </summary>
		private string m_currentGroup;

		/// <summary>
		/// The individual buttons in the currently-selected sidebar group.
		/// </summary>
		private IList<ButtonControl> m_elementButtons = new List<ButtonControl>();

		/// <summary>
		/// The buttons for the sidebar groups.
		/// </summary>
		private IList<ButtonControl> m_groupButtons = new List<ButtonControl>();

		/// <summary>
		/// The groups of element specifiers that describe the functionality of the sidebar.
		/// </summary>
		private readonly IDictionary<string,List<ElementSpecifier>> m_groups = new Dictionary<string,List<ElementSpecifier>>();

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "SidebarState"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar state component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public SidebarStateComponent(XElement componentElt)
		:	base(componentElt)
		{
			LoadEntityGroups(componentElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after the component containing this entity is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			// TODO
			Entity.AddChild(new ButtonControl("blah", "sidebarelement_Dwelling", new Viewport(Entity.Viewport.X + 20, Entity.Viewport.Y + 20, 50, 50)));
			base.AfterAdd();
		}

		/// <summary>
		/// Called just before the component containing this entity is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();
			// TODO
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Load the groups of entity that the player can manipulate from the XML representing the component.
		/// </summary>
		/// <param name="componentElt">The root node of the component's XML representation.</param>
		private void LoadEntityGroups(XElement componentElt)
		{
			foreach(XElement group in componentElt.Elements("group"))
			{
				string groupName = group.Attribute("name").Value;
				m_groups[groupName] = new List<ElementSpecifier>();

				foreach(XElement element in group.Elements("element"))
				{
					XAttribute nameAttribute = element.Attribute("name");
					XAttribute toolAttribute = element.Attribute("tool");
					if(nameAttribute == null) continue;

					m_groups[groupName].Add(new ElementSpecifier
					{
						Name = nameAttribute.Value,
						Tool = toolAttribute != null ? toolAttribute.Value : (string)null
					});
				}
			}
		}

		#endregion
	}
}
