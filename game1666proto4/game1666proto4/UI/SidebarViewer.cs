/***
 * game1666proto4: SidebarViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using game1666proto4.Common.Graphics;
using game1666proto4.Common.Input;
using game1666proto4.GameModel.Entities;
using game1666proto4.UI.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666proto4.UI
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class SidebarViewer : CompositeVisibleEntity
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
		private IList<Button> m_elementButtons = new List<Button>();

		/// <summary>
		/// The buttons for the sidebar groups.
		/// </summary>
		private IList<Button> m_groupButtons = new List<Button>();

		/// <summary>
		/// The groups of element specifiers that describe the functionality of the sidebar.
		/// </summary>
		private readonly IDictionary<string,List<ElementSpecifier>> m_groups = new Dictionary<string,List<ElementSpecifier>>();

		/// <summary>
		/// The playing area whose entities this sidebar is used to manipulate.
		/// </summary>
		private readonly IPlayingArea m_playingArea;

		/// <summary>
		/// The sprite batch used when drawing the sidebar.
		/// </summary>
		private readonly SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture used when drawing the sidebar.
		/// </summary>
		private readonly Texture2D m_texture;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The sub-entities contained within the composite.
		/// </summary>
		public override IEnumerable<dynamic> Children
		{
			get
			{
				foreach(Button button in m_elementButtons)	yield return button;
				foreach(Button button in m_groupButtons)	yield return button;
			}
		}

		/// <summary>
		/// The state shared between the visible entities that together make up the game view - e.g. things like the currently active tool.
		/// </summary>
		public GameViewState GameViewState { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		/// <param name="world">The world that is being viewed.</param>
		public SidebarViewer(XElement entityElt, World world)
		:	base(entityElt, world)
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_playingArea = World.GetEntityByPath(Properties["PlayingArea"]);
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("Textures/sidebarbackground");
			Viewport = Properties["Viewport"];

			LoadEntityGroups(entityElt);
			CreateGroupButtons();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the viewer based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			// No-op
		}

		/// <summary>
		/// Draws the sidebar for the playing area.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Viewport;
			Renderer.Setup2D();

			m_spriteBatch.Begin();
			m_spriteBatch.Draw(m_texture, new Rectangle(0, 0, Viewport.Width, Viewport.Height), Color.White);
			m_spriteBatch.End();

			// Draw the sidebar buttons.
			base.Draw();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

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
		private static List<Button> CreateButtons(Viewport buttonsViewport, int columns, IList<ButtonSpecifier> buttonSpecifiers)
		{
			var buttons = new List<Button>();

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
				var button = new Button(bs.TextureName, ConstructButtonViewport(buttonsViewport, layout, row, column));
				button.IsHighlighted = bs.IsHighlighted;
				button.MousePressedHook += bs.MousePressedHook;

				// Add the button to the list.
				buttons.Add(button);

				++buttonIndex;
			}

			return buttons;
		}

		/// <summary>
		/// Creates buttons for the individual entities in the specified group.
		/// </summary>
		/// <param name="group">The group.</param>
		private void CreateElementButtons(string group)
		{
			// Set the current group.
			m_currentGroup = group;

			// Create the button specifiers.
			var buttonSpecifiers = new List<ButtonSpecifier>();
			buttonSpecifiers.AddRange(m_groups[group].Select(element => new ButtonSpecifier
			{
				IsHighlighted		= () => GameViewState.Tool != null && GameViewState.Tool.Name.EndsWith(":" + element.Name),
				MousePressedHook	= UseToolHook(element.Tool, element.Name),
				TextureName			= "sidebarelement_" + element.Name
			}));

			// Create the buttons themselves.
			m_elementButtons = CreateButtons(ElementButtonsViewport(), 3, buttonSpecifiers);
		}

		/// <summary>
		/// Creates buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private void CreateGroupButtons()
		{
			// Create the button specifiers.
			var buttonSpecifiers = new List<ButtonSpecifier>();
			buttonSpecifiers.AddRange(m_groups.Keys.Select(group => new ButtonSpecifier
			{
				IsHighlighted		= () => m_currentGroup == group,
				MousePressedHook	= state => { GameViewState.Tool = null; CreateElementButtons(group); },
				TextureName			= "sidebargroup_" + group
			}));

			// Create the buttons themselves.
			m_groupButtons = CreateButtons(GroupButtonsViewport(), 2, buttonSpecifiers);
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
			int margin = (int)(Viewport.Width / 10);
			return new Viewport
			{
				X = Viewport.X + margin,
				Y = Viewport.Y + margin + Viewport.Height / 3,
				Width = Viewport.Width - 2 * margin,
				Height = Viewport.Height / 3 - 2 * margin
			};
		}

		/// <summary>
		/// Construct the viewport in which to place group buttons.
		/// </summary>
		/// <returns>The constructed viewport.</returns>
		private Viewport GroupButtonsViewport()
		{
			// Use the top third of the sidebar as the area in which to place the group buttons.
			int margin = (int)(Viewport.Width / 10);
			return new Viewport
			{
				X = Viewport.X + margin,
				Y = Viewport.Y + margin,
				Width = Viewport.Width - 2 * margin,
				Height = Viewport.Height / 3 - 2 * margin
			};
		}

		/// <summary>
		/// Load the groups of entity that the player can manipulate from the XML representing the viewer.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		private void LoadEntityGroups(XElement entityElt)
		{
			foreach(XElement group in entityElt.Elements("group"))
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
			string toolTypename = "game1666proto4.UI.Tools." + tool;
			Type toolType = Type.GetType(toolTypename);
			if(toolType == null)
			{
				throw new InvalidDataException("No such tool type: " + toolTypename);
			}

			// If the type was found, create a mouse event that will set the current tool to a
			// new instance of the type when invoked.
			return state => GameViewState.Tool = Activator.CreateInstance(toolType, name, m_playingArea) as ITool;
		}

		#endregion
	}
}
