/***
 * game1666: PlayRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.Entities;
using game1666.Common.UI;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Terrains;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of this class draws a play viewer that shows the contents of a playing area (such as the world or a settlement).
	/// </summary>
	sealed class PlayRenderingComponent : RenderingComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		private readonly Camera m_camera;

		/// <summary>
		/// The current projection matrix.
		/// </summary>
		private Matrix m_matProjection;

		/// <summary>
		/// The current view matrix.
		/// </summary>
		private Matrix m_matView;

		/// <summary>
		/// The current world matrix.
		/// </summary>
		private Matrix m_matWorld;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a play rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayRenderingComponent(XElement componentElt)
		:	base(componentElt)
		{
			m_camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), Vector3.UnitZ);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the play viewer of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			// Look up the target entity.
			IUIEntity gameView = Entity.Parent;
			string targetPath = gameView.Properties["Target"];
			if(targetPath == "./settlement:Home") targetPath = Entity.World.Properties["HomeSettlement"];
			IBasicEntity targetEntity = Entity.World.GetEntityByAbsolutePath(targetPath);
			if(targetEntity == null) return;

			// Prepare for rendering.
			Renderer.GraphicsDevice.Viewport = Entity.Viewport;
			Renderer.Setup3D();
			SetupMatrices();

			// Draw the terrain.
			var internalComponent = targetEntity.GetComponent<PlayingAreaComponent>(PlayingAreaComponent.StaticGroup);
			if(internalComponent != null) DrawTerrain(internalComponent.Terrain);

			// TODO
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Draws a terrain.
		/// </summary>
		/// <param name="terrain">The terrain to draw.</param>
		private void DrawTerrain(Terrain terrain)
		{
			var effect = Renderer.Content.Load<Effect>("Effects/TerrainMultitexture");
			effect.Parameters["World"].SetValue(m_matWorld);
			effect.Parameters["View"].SetValue(m_matView);
			effect.Parameters["Projection"].SetValue(m_matProjection);
			effect.Parameters["Texture0"].SetValue(Renderer.Content.Load<Texture2D>("Textures/grass"));
			effect.Parameters["Texture1"].SetValue(Renderer.Content.Load<Texture2D>("Textures/snow"));
			effect.Parameters["TransitionHalfWidth"].SetValue(terrain.TransitionHalfWidth);
			effect.Parameters["TransitionHeight"].SetValue(terrain.TransitionHeight);
			Renderer.DrawTriangleList(terrain.VertexBuffer, terrain.IndexBuffer, effect);
		}

		/// <summary>
		/// Sets up the world, view and projection matrices ready for rendering.
		/// </summary>
		private void SetupMatrices()
		{
			m_matProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)Entity.Viewport.Width / Entity.Viewport.Height, 0.1f, 1000.0f);
			m_matView = Matrix.CreateLookAt(m_camera.Position, m_camera.Position + m_camera.N, m_camera.V);
			m_matWorld = Matrix.Identity;
		}

		#endregion
	}
}
