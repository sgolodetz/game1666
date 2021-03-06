/***
 * game1666: Game.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using System.Xml.XPath;
using game1666.Common.Persistence;
using game1666.Common.Tasks;
using game1666.Common.UI;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Components;
using game1666.GameModel.Entities.Lifetime;
using game1666.GameModel.Entities.PlacementStrategies;
using game1666.GameModel.Entities.Tasks;
using game1666.GameModel.Terrains;
using game1666.GameUI;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666
{
	/// <summary>
	/// An instance of this class represents the game itself.
	/// </summary>
	sealed class Game : Microsoft.Xna.Framework.Game
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The manager containing the various different views for the game.
		/// </summary>
		private GameViewManager m_gameViewManager;

		/// <summary>
		/// Handles the configuration and management of the graphics device.
		/// </summary>
		private readonly GraphicsDeviceManager m_graphicsDeviceManager;

		/// <summary>
		/// The game world (the top-level entity in the game model tree).
		/// </summary>
		private ModelEntity m_world;

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Sets up the graphics device and window, and sets the root directory for content.
		/// </summary>
		public Game()
		{
			m_graphicsDeviceManager = new GraphicsDeviceManager(this);
			if(GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
			{
				m_graphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
			}
			m_graphicsDeviceManager.PreferredBackBufferWidth = 1024;
			m_graphicsDeviceManager.PreferredBackBufferHeight = 768;
			m_graphicsDeviceManager.SynchronizeWithVerticalRetrace = true;

			this.IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			m_gameViewManager.Draw();

			base.Draw(gameTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content. Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Set up the renderer.
			Renderer.Content = Content;
			Renderer.GraphicsDevice = GraphicsDevice;

			// Register special XML elements with the object persister.
			ObjectPersister.RegisterSpecialElement("cmpButtonInteraction", typeof(ButtonInteractionComponent));
			ObjectPersister.RegisterSpecialElement("cmpButtonRendering", typeof(ButtonRenderingComponent));
			ObjectPersister.RegisterSpecialElement("cmpCompositeInteraction", typeof(CompositeInteractionComponent));
			ObjectPersister.RegisterSpecialElement("cmpCompositeRendering", typeof(CompositeRenderingComponent));
			ObjectPersister.RegisterSpecialElement("cmpGameViewState", typeof(GameViewStateComponent));
			ObjectPersister.RegisterSpecialElement("cmpHome", typeof(HomeComponent));
			ObjectPersister.RegisterSpecialElement("cmpMobile", typeof(MobileComponent));
			ObjectPersister.RegisterSpecialElement("cmpPerson", typeof(PersonComponent));
			ObjectPersister.RegisterSpecialElement("cmpPlaceable", typeof(PlaceableComponent));
			ObjectPersister.RegisterSpecialElement("cmpPlayingArea", typeof(PlayingAreaComponent));
			ObjectPersister.RegisterSpecialElement("cmpPlayInteraction", typeof(PlayInteractionComponent));
			ObjectPersister.RegisterSpecialElement("cmpPlayRendering", typeof(PlayRenderingComponent));
			ObjectPersister.RegisterSpecialElement("cmpPlayState", typeof(PlayStateComponent));
			ObjectPersister.RegisterSpecialElement("cmpSidebarRendering", typeof(SidebarRenderingComponent));
			ObjectPersister.RegisterSpecialElement("cmpSidebarState", typeof(SidebarStateComponent));
			ObjectPersister.RegisterSpecialElement("cmpSpawner", typeof(SpawnerComponent));
			ObjectPersister.RegisterSpecialElement("cmpTraversable", typeof(TraversableComponent));
			ObjectPersister.RegisterSpecialElement("entity", typeof(ModelEntity));
			ObjectPersister.RegisterSpecialElement("footprint", typeof(Footprint));
			ObjectPersister.RegisterSpecialElement("homeblueprint", typeof(HomeBlueprint));
			ObjectPersister.RegisterSpecialElement("mobileblueprint", typeof(MobileBlueprint));
			ObjectPersister.RegisterSpecialElement("placeableblueprint", typeof(PlaceableBlueprint));
			ObjectPersister.RegisterSpecialElement("placementstrategyRequireFlatGround", typeof(PlacementStrategyRequireFlatGround));
			ObjectPersister.RegisterSpecialElement("spawnerblueprint", typeof(SpawnerBlueprint));
			ObjectPersister.RegisterSpecialElement("terrain", typeof(Terrain));
			ObjectPersister.RegisterSpecialElement("tskEnterEntity", typeof(TaskEnterEntity));
			ObjectPersister.RegisterSpecialElement("tskExitEntity", typeof(TaskExitEntity));
			ObjectPersister.RegisterSpecialElement("tskGoToALocalPosition", typeof(TaskGoToALocalPosition));
			ObjectPersister.RegisterSpecialElement("tskGoToAPosition", typeof(TaskGoToAPosition));
			ObjectPersister.RegisterSpecialElement("tskGoToEntity", typeof(TaskGoToEntity));
			ObjectPersister.RegisterSpecialElement("tskGoToLocalEntity", typeof(TaskGoToLocalEntity));
			ObjectPersister.RegisterSpecialElement("tskLeaveEntity", typeof(TaskLeaveEntity));
			ObjectPersister.RegisterSpecialElement("tskPrioritised", typeof(PrioritisedTask));
			ObjectPersister.RegisterSpecialElement("tskPriorityQueue", typeof(PriorityQueueTask));
			ObjectPersister.RegisterSpecialElement("tskSequence", typeof(SequenceTask));
			ObjectPersister.RegisterSpecialElement("uientity", typeof(UIEntity));

			// Load the world from an XML file.
			m_world = LoadWorldFromFile(@"Content\PathfindingWorld.xml");

			// Load the game views from the game configuration file.
			var doc = XDocument.Load(@"Content\GameConfig.xml");
			m_gameViewManager = new GameViewManager(doc.XPathSelectElement("config/gameviews"), m_world);

			base.Initialize();
		}

		/// <summary>
		/// Called once per game to allow content to be loaded.
		/// </summary>
		protected override void LoadContent()
		{
			// Pre-load content.
			Content.Load<Texture2D>("Textures/grass");
			Content.Load<Texture2D>("Textures/snow");
		}

		/// <summary>
		/// Called once per game to allow content to be unloaded.
		/// </summary>
		protected override void UnloadContent()
		{
			Content.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyState = Keyboard.GetState();
			if(keyState.IsKeyDown(Keys.Escape))	Exit();
			if(keyState.IsKeyDown(Keys.F1))	m_gameViewManager.CurrentView = "gameview:City";
			if(keyState.IsKeyDown(Keys.F2))	m_gameViewManager.CurrentView = "gameview:World";

			MouseEventManager.Update();
			m_gameViewManager.Update(gameTime);
			m_world.Update(gameTime);

			base.Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Loads a world from an XML file.
		/// </summary>
		/// <param name="filename">The name of the XML file.</param>
		/// <returns>The loaded world.</returns>
		private static ModelEntity LoadWorldFromFile(string filename)
		{
			XDocument doc = XDocument.Load(filename);
			XElement entityElt = doc.Element("entity");
			ModelEntity world = new ModelEntity(entityElt);

			new ModelContextComponent
			(
				new ModelEntityDestructionManager(),
				new TaskFactory()
			).AddToEntity(world);

			world.AddDescendantsFromXML(entityElt);
			return world;
		}

		#endregion
	}
}
