/***
 * game1666: SidebarStateComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameUI.Entities.Components.Button;
using game1666.GameUI.Entities.Components.Common;
using game1666.GameUI.Entities.Components.GameView;
using game1666.GameUI.Tools;
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
		/// The individual buttons in the currently-selected sidebar group.
		/// </summary>
		private IList<ButtonControl> ElementButtons
		{
			get
			{
				return m_elementButtons;
			}

			set
			{
				RemoveElementButtons();
				m_elementButtons = value;
				AddElementButtons();
			}
		}

		/// <summary>
		/// The buttons for the sidebar groups.
		/// </summary>
		private IList<ButtonControl> GroupButtons
		{
			get
			{
				return m_groupButtons;
			}

			set
			{
				RemoveGroupButtons();
				m_groupButtons = value;
				AddGroupButtons();
			}
		}

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "SidebarState"; } }

		/// <summary>
		/// The currently active tool (e.g. an entity placement tool), or null if no tool is active.
		/// </summary>
		public ITool Tool
		{
			get	{ return Entity.Parent.GetComponent(GameViewStateComponent.StaticGroup).Tool; }
			set	{ Entity.Parent.GetComponent(GameViewStateComponent.StaticGroup).Tool = value; }
		}

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
		/// Called just after the entity containing this component is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			ReplaceGroupButtons();

			base.AfterAdd();
		}

		/// <summary>
		/// Called just before the entity containing this component is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			base.BeforeRemove();

			RemoveElementButtons();
			RemoveGroupButtons();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Adds the current list of element buttons to the sidebar viewer.
		/// </summary>
		private void AddElementButtons()
		{
			foreach(var button in m_elementButtons)
			{
				Entity.AddChild(button);
			}
		}

		/// <summary>
		/// Adds the current list of group buttons to the sidebar viewer.
		/// </summary>
		private void AddGroupButtons()
		{
			foreach(var button in m_groupButtons)
			{
				Entity.AddChild(button);
			}
		}

		/// <summary>
		/// Constructs the viewport for a button, based on the viewport of the set of buttons containing it, the determined
		/// grid layout for the set of buttons, and the row and column of the grid in which the button lies.
		/// </summary>
		/// <param name="buttonsViewport">The viewport of the containing set of buttons.</param>
		/// <param name="layout">The determined grid layout for the set of buttons.</param>
		/// <param name="row">The grid row in which the button lies.</param>
		/// <param name="column">The grid column in which the button lies.</param>
		/// <returns>The viewport for the button.</returns>
		private static Viewport ConstructButtonViewport(Viewport buttonsViewport, ButtonLayout layout, int row, int column)
		{
			return new Viewport
			{
				X = buttonsViewport.X + layout.XOffset + column * layout.ColumnWidth,
				Y = buttonsViewport.Y + row * layout.RowHeight,
				Width = layout.ButtonWidth,
				Height = layout.ButtonHeight
			};
		}

		/// <summary>
		/// Creates buttons to fill a given viewport, based on a list of button specifiers.
		/// </summary>
		/// <param name="buttonsViewport">The viewport that the buttons are to fill.</param>
		/// <param name="columns">The number of columns in which the buttons should be laid out.</param>
		/// <param name="buttonSpecifiers">The button specifiers.</param>
		/// <returns>The created buttons.</returns>
		private static List<ButtonControl> CreateButtons(Viewport buttonsViewport, int columns, IList<ButtonSpecifier> buttonSpecifiers)
		{
			var buttons = new List<ButtonControl>();

			// If there are no button specifiers, early out and return an empty list of buttons.
			if(buttonSpecifiers.Count == 0) return buttons;

			// Determine how the buttons should be laid out.
			var layout = DetermineButtonLayout(buttonsViewport, buttonSpecifiers.Count, columns);

			// Construct the buttons (using the supplied specifiers) and add them to the buttons list.
			int buttonIndex = 0;
			foreach(ButtonSpecifier bs in buttonSpecifiers)
			{
				// Work out in which row and column the button lies.
				int column = buttonIndex % layout.Columns;
				int row = buttonIndex / layout.Columns;

				// Construct the button and set its handlers.
				var button = new ButtonControl(bs.TextureName, ConstructButtonViewport(buttonsViewport, layout, row, column));
				var buttonInteractor = button.GetComponent(ButtonInteractionComponent.StaticGroup);
				var buttonRenderer = button.GetComponent(ButtonRenderingComponent.StaticGroup);
				if(buttonInteractor != null) buttonInteractor.MousePressedHook += bs.MousePressedHook;
				if(buttonRenderer != null) buttonRenderer.IsHighlighted = bs.IsHighlighted;

				// Add the button to the list.
				buttons.Add(button);

				++buttonIndex;
			}

			return buttons;
		}

		/// <summary>
		/// Determines how a set of buttons should be laid out.
		/// </summary>
		/// <param name="buttonsViewport">The viewport for the set of buttons as a whole.</param>
		/// <param name="buttonCount">The number of buttons that need to be laid out.</param>
		/// <param name="columns">The number of columns to use for the button grid.</param>
		/// <returns>The determined button layout.</returns>
		private static ButtonLayout DetermineButtonLayout(Viewport buttonsViewport, int buttonCount, int columns)
		{
			var result = new ButtonLayout();

			// Work out the dimensions of each button.
			result.Columns = columns;
			result.Rows = (buttonCount + result.Columns - 1) / result.Columns;	// note: this rounds up when the last row is incomplete
			result.ButtonWidth = Math.Min((buttonsViewport.Width + HORIZONTAL_SPACING) / result.Columns - HORIZONTAL_SPACING, MAX_BUTTON_WIDTH);
			result.ButtonHeight = Math.Min((buttonsViewport.Height + VERTICAL_SPACING) / result.Rows - VERTICAL_SPACING, MAX_BUTTON_HEIGHT);

			if(ENSURE_SQUARE_BUTTONS)
			{
				// Ensure that the buttons are square.
				result.ButtonWidth = result.ButtonHeight = Math.Min(result.ButtonWidth, result.ButtonHeight);
			}

			// Work out the width of columns and the height of rows in the button grid.
			result.ColumnWidth = result.ButtonWidth + HORIZONTAL_SPACING;
			result.RowHeight = result.ButtonHeight + VERTICAL_SPACING;

			// Determine the top-left button's x offset from the top-left of the group buttons viewport.
			// This is calculated so as to horizontally centre the buttons in the group buttons viewport.
			result.XOffset = (buttonsViewport.Width - (result.Columns * result.ColumnWidth - HORIZONTAL_SPACING)) / 2;

			return result;
		}

		/// <summary>
		/// Construct the viewport in which to place element buttons.
		/// </summary>
		/// <returns>The constructed viewport.</returns>
		private Viewport ElementButtonsViewport()
		{
			// Use the middle third of the sidebar as the area in which to place the element buttons.
			int margin = (int)(Entity.Viewport.Width / 10);
			return new Viewport
			{
				X = Entity.Viewport.X + margin,
				Y = Entity.Viewport.Y + margin + Entity.Viewport.Height / 3,
				Width = Entity.Viewport.Width - 2 * margin,
				Height = Entity.Viewport.Height / 3 - 2 * margin
			};
		}

		/// <summary>
		/// Construct the viewport in which to place group buttons.
		/// </summary>
		/// <returns>The constructed viewport.</returns>
		private Viewport GroupButtonsViewport()
		{
			// Use the top third of the sidebar as the area in which to place the group buttons.
			int margin = (int)(Entity.Viewport.Width / 10);
			return new Viewport
			{
				X = Entity.Viewport.X + margin,
				Y = Entity.Viewport.Y + margin,
				Width = Entity.Viewport.Width - 2 * margin,
				Height = Entity.Viewport.Height / 3 - 2 * margin
			};
		}

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

		/// <summary>
		/// Removes the buttons for the individual entities in the current group.
		/// </summary>
		private void RemoveElementButtons()
		{
			foreach(var button in m_elementButtons)
			{
				Entity.RemoveChild(button);
			}
		}

		/// <summary>
		/// Removes the buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private void RemoveGroupButtons()
		{
			foreach(var button in m_groupButtons)
			{
				Entity.RemoveChild(button);
			}
		}

		/// <summary>
		/// Replaces the current list of element buttons with the buttons for the specified group,
		/// and updates the sidebar viewer accordingly.
		/// </summary>
		/// <param name="group">The group.</param>
		private void ReplaceElementButtons(string group)
		{
			// Set the current group.
			m_currentGroup = group;

			// Create the button specifiers.
			var buttonSpecifiers = new List<ButtonSpecifier>();
			buttonSpecifiers.AddRange(m_groups[group].Select(element => new ButtonSpecifier
			{
				IsHighlighted		= () => Tool != null && Tool.Name.EndsWith(":" + element.Name),
				MousePressedHook	= UseToolHook(element.Tool, element.Name),
				TextureName			= "sidebarelement_" + element.Name
			}));

			// Create the buttons themselves and replace the element buttons currently in the sidebar viewer.
			ElementButtons = CreateButtons(ElementButtonsViewport(), 3, buttonSpecifiers);
		}

		/// <summary>
		/// Replaces the current list of buttons for the groups of entity that the player can manipulate,
		/// and updates the sidebar viewer accordingly.
		/// </summary>
		private void ReplaceGroupButtons()
		{
			// Create the button specifiers.
			var buttonSpecifiers = new List<ButtonSpecifier>();
			buttonSpecifiers.AddRange(m_groups.Keys.Select(group => new ButtonSpecifier
			{
				IsHighlighted		= () => m_currentGroup == group,
				MousePressedHook	= state => { Tool = null; ReplaceElementButtons(group); },
				TextureName			= "sidebargroup_" + group
			}));

			// Create the buttons themselves and replace the group buttons currently in the sidebar viewer.
			GroupButtons = CreateButtons(GroupButtonsViewport(), 2, buttonSpecifiers);
		}

		/// <summary>
		/// Returns a mouse event that sets the current tool to a new instance of the specified type when invoked.
		/// </summary>
		/// <param name="tool">The (unqualified) name of the tool type, e.g. "EntityPlacementTool".</param>
		/// <param name="name">The name argument to pass to the tool's constructor, e.g. "Dwelling".</param>
		/// <returns>A mouse event that sets the current tool to a new instance of the specified type when invoked.</returns>
		private MouseEvent UseToolHook(string tool, string name)
		{
			// If no tool was specified, return a mouse event that will do nothing when called.
			if(tool == null)
			{
				return state => {};
			}

			// If a tool was specified, look up its type.
			string toolTypename = "game1666.GameUI.Tools." + tool;
			Type toolType = Type.GetType(toolTypename);
			if(toolType == null)
			{
				//throw new InvalidDataException("No such tool type: " + toolTypename);
			}

			// If the type was found, create a mouse event that will set the current tool to a
			// new instance of the type when invoked.
			return state => {};//Tool = Activator.CreateInstance(toolType, name, m_playingArea) as ITool;
		}

		#endregion
	}
}
