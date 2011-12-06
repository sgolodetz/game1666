/***
 * game1666proto4: SidebarViewer.cs
 * Copyright 2011. All rights reserved.
 ***/

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
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The buttons for the individual entities that the player can manipulate.
		/// </summary>
		private IList<Button> m_elementButtons = new List<Button>();

		/// <summary>
		/// The buttons for the groups of entity that the player can manipulate.
		/// </summary>
		private IList<Button> m_groupButtons = new List<Button>();

		/// <summary>
		/// The groups of entity that the player can manipulate.
		/// </summary>
		private readonly IDictionary<string,List<string>> m_groups = new Dictionary<string,List<string>>();

		/// <summary>
		/// The playing area whose entities this sidebar is used to manipulate.
		/// </summary>
		private PlayingArea m_playingArea;

		/// <summary>
		/// The sprite batch used when drawing the sidebar.
		/// </summary>
		private SpriteBatch m_spriteBatch;

		/// <summary>
		/// The texture used when drawing the sidebar.
		/// </summary>
		private Texture2D m_texture;

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
		/// Constructs a sidebar viewer for the specified playing area.
		/// </summary>
		/// <param name="playingAreaSpecifier">The entity path of the specified playing area.</param>
		/// <param name="viewportSpecifier">A string specifying the viewport into which to draw the sidebar.</param>
		public SidebarViewer(string playingAreaSpecifier, string viewportSpecifier)
		{
			Properties["PlayingArea"] = playingAreaSpecifier;
			Properties["Viewport"] = viewportSpecifier;
			Initialise();
		}

		/// <summary>
		/// Constructs a sidebar viewer from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the viewer's XML representation.</param>
		public SidebarViewer(XElement entityElt)
		:	base(entityElt)
		{
			// Initialise the sidebar view based on its properties.
			Initialise();

			// Load in the entity groups.
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

			// TEMPORARY: This is just a test button.
			var button = new Button("landscape", new Viewport { X = Viewport.X + 10, Y = Viewport.Y + 10, Width = Viewport.Width - 20, Height = 30 });
			button.MousePressedHook += state => System.Console.WriteLine("Clicked");
			m_groupButtons.Add(button);
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
		/// Initialises the viewer based on its properties.
		/// </summary>
		private void Initialise()
		{
			// Enforce the postcondition.
			Contract.Ensures(m_playingArea != null);

			m_playingArea = SceneGraph.GetEntityByPath(Properties["PlayingArea"]);
			m_spriteBatch = new SpriteBatch(Renderer.GraphicsDevice);
			m_texture = Renderer.Content.Load<Texture2D>("landscape");
			Viewport = ViewUtil.ParseViewportSpecifier(Properties["Viewport"]);
		}

		#endregion
	}
}
