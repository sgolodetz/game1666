/***
 * game1666proto4: SidebarViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class is used to show a sidebar for a playing area,
	/// allowing the user to place / remove entities.
	/// </summary>
	sealed class SidebarViewer : CompositeViewEntity
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
		/// An instance of this struct represents the way in which a set of buttons should be laid out (in a grid).
		/// </summary>
		private struct ButtonLayout
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
		};

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buttons for the individual entities in the current group that the player can manipulate.
		/// </summary>
		private readonly IList<Button> m_elementButtons = new List<Button>();

		/// <summary>
		/// The buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private readonly IList<Button> m_groupButtons = new List<Button>();

		/// <summary>
		/// The groups of entity that the player can manipulate.
		/// </summary>
		private readonly IDictionary<string,List<string>> m_groups = new Dictionary<string,List<string>>();

		/// <summary>
		/// The playing area whose entities this sidebar is used to manipulate.
		/// </summary>
		private readonly PlayingArea m_playingArea;

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
		protected override IEnumerable<ViewEntity> Children
		{
			get
			{
				foreach(Button button in m_elementButtons)	yield return button;
				foreach(Button button in m_groupButtons)	yield return button;
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a sidebar viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		public SidebarViewer(XElement entityElt)
		:	base(entityElt)
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("sidebarbackground");
			Viewport = ViewUtil.ParseViewportSpecifier(Properties["Viewport"]);

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
		public override void AddEntityDynamic(dynamic entity)
		{
			// No-op
		}

		/// <summary>
		/// Draws the sidebar for the playing area.
		/// </summary>
		public override void Draw()
		{
			Renderer.GraphicsDevice.Viewport = Viewport;
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
		/// <returns></returns>
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
		/// Creates buttons for the individual entities in the specified group.
		/// </summary>
		/// <param name="group">The group.</param>
		private void CreateElementButtons(string group)
		{
			// Clear the existing element buttons list ready to fill it with elements from the new group.
			m_elementButtons.Clear();

			// Use the middle third of the sidebar as the area in which to place the element buttons.
			int margin = (int)(Viewport.Width / 10);
			var elementButtonsViewport = new Viewport
			{
				X = Viewport.X + margin,
				Y = Viewport.Y + margin + Viewport.Height / 3,
				Width = Viewport.Width - 2 * margin,
				Height = Viewport.Height / 3 - 2 * margin
			};

			// Determine how the buttons should be laid out.
			var layout = DetermineButtonLayout(elementButtonsViewport, m_groups[group].Count, 3);

			// Construct the buttons and add them to the element buttons list.
			int buttonIndex = 0;
			foreach(string element in m_groups[group])
			{
				// Work out in which row and column the button lies.
				int column = buttonIndex % layout.Columns;
				int row = buttonIndex / layout.Columns;

				// Construct the button and set its mouse pressed handler.
				var button = new Button("sidebarelement_" + element, ConstructButtonViewport(elementButtonsViewport, layout, row, column));
				string elementCopy = element;
				button.MousePressedHook += state => Console.WriteLine("Clicked Element: " + elementCopy);

				// Add the button to the list.
				m_elementButtons.Add(button);

				++buttonIndex;
			}
		}

		/// <summary>
		/// Creates buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private void CreateGroupButtons()
		{
			// Use the top third of the sidebar as the area in which to place the group buttons.
			int margin = (int)(Viewport.Width / 10);
			var groupButtonsViewport = new Viewport
			{
				X = Viewport.X + margin,
				Y = Viewport.Y + margin,
				Width = Viewport.Width - 2 * margin,
				Height = Viewport.Height / 3 - 2 * margin
			};

			// Determine how the buttons should be laid out.
			var layout = DetermineButtonLayout(groupButtonsViewport, m_groups.Keys.Count, 2);

			// Construct the buttons and add them to the group buttons list.
			int buttonIndex = 0;
			foreach(string group in m_groups.Keys)
			{
				// Work out in which row and column the button lies.
				int column = buttonIndex % layout.Columns;
				int row = buttonIndex / layout.Columns;

				// Construct the button and set its mouse pressed handler.
				var button = new Button("sidebargroup_" + group, ConstructButtonViewport(groupButtonsViewport, layout, row, column));
				string groupCopy = group;
				button.MousePressedHook += state => CreateElementButtons(groupCopy);

				// Add the button to the list.
				m_groupButtons.Add(button);

				++buttonIndex;
			}
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
		/// Load the groups of entity that the player can manipulate from the XML representing the viewer.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		private void LoadEntityGroups(XElement entityElt)
		{
			foreach(XElement group in entityElt.Elements("group"))
			{
				string groupName = group.Attribute("name").Value;
				m_groups[groupName] = new List<string>();

				foreach(XElement element in group.Elements("element"))
				{
					string elementName = element.Attribute("name").Value;
					m_groups[groupName].Add(elementName);
				}
			}
		}

		#endregion
	}
}
