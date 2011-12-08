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

		private const int HORIZONTAL_SPACING = 10;
		private const int VERTICAL_SPACING = 10;

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buttons for the individual entities that the player can manipulate.
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
			m_texture = Renderer.Content.Load<Texture2D>("landscape");
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
		/// Creates the buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private void CreateGroupButtons()
		{
			// Use the top third of the sidebar as the area in which to place the group buttons.
			var groupButtonsViewport = new Viewport { X = Viewport.X, Y = Viewport.Y, Width = Viewport.Width, Height = Viewport.Height / 3 };

			// Work out how many buttons are needed, and their dimensions.
			int groupCount = m_groups.Keys.Count;
			int columns = 2;
			int rows = (groupCount + columns - 1) / columns;	// note: this has the effect of rounding up when the last row is incomplete
			int buttonWidth = Math.Min((groupButtonsViewport.Width - HORIZONTAL_SPACING) / columns - HORIZONTAL_SPACING, MAX_BUTTON_WIDTH);
			int buttonHeight = Math.Min((groupButtonsViewport.Height - VERTICAL_SPACING) / rows - VERTICAL_SPACING, MAX_BUTTON_HEIGHT);

			if(ENSURE_SQUARE_BUTTONS)
			{
				// Ensure that the buttons are square.
				buttonWidth = buttonHeight = Math.Min(buttonWidth, buttonHeight);
			}

			// Work out the width of columns and the height of rows in the button grid.
			int columnWidth = buttonWidth + HORIZONTAL_SPACING;
			int rowHeight = buttonHeight + VERTICAL_SPACING;

			// Determine the top-left button's x and y offsets from the top-left of the group buttons viewport.
			// Note that the x offset is calculated so as to centre the buttons in the group buttons viewport.
			int xOffset = (groupButtonsViewport.Width - (columns * columnWidth - HORIZONTAL_SPACING)) / 2;
			int yOffset = VERTICAL_SPACING;

			// Construct the buttons and add them to the group button list.
			int i = 0;
			foreach(string group in m_groups.Keys)
			{
				// Work out which row and column we're in.
				int column = i % columns;
				int row = i / columns;

				// TODO: Look up the actual button texture for the specified group.
				string textureName = "landscape";

				// Construct the viewport for the button based on its location in the grid.
				var viewport = new Viewport
				{
					X = groupButtonsViewport.X + xOffset + column * columnWidth,
					Y = groupButtonsViewport.Y + yOffset + row * rowHeight,
					Width = buttonWidth,
					Height = buttonHeight
				};

				// Construct the actual button and set its mouse pressed handler.
				var button = new Button(textureName, viewport);
				string groupCopy = group;
				button.MousePressedHook += state => System.Console.WriteLine("Clicked Button: " + groupCopy);

				// Add the button to the list.
				m_groupButtons.Add(button);

				++i;
			}
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
