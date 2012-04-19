/***
 * game1666: PlayRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameModel.Entities;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Entities.Components.Rendering;
using game1666.GameModel.Terrains;
using game1666.GameUI.Entities.Components.Interaction;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of this class draws a play viewer that shows the contents of a playing area (such as the world or a settlement).
	/// </summary>
	sealed class PlayRenderingComponent : ControlRenderingComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The basic effect for rendering entities.
		/// </summary>
		private readonly BasicEffect m_entityEffect = new BasicEffect(Renderer.GraphicsDevice);

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

		/// <summary>
		/// The basic effect for rendering the terrain quadtree.
		/// </summary>
		private readonly BasicEffect m_terrainQuadtreeEffect = new BasicEffect(Renderer.GraphicsDevice);

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
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the play viewer of which this component is a part.
		/// </summary>
		public override void Draw()
		{
			// Look up the target entity of the game view containing the play viewer.
			IModelEntity targetEntity = UIEntityComponentUtil.GetTarget(Entity.Parent);
			if(targetEntity == null) return;

			// Prepare for rendering.
			Renderer.GraphicsDevice.Viewport = Entity.Viewport;
			Renderer.Setup3D();
			SetupMatrices();

			// Draw the terrain.
			var internalComponent = targetEntity.GetComponent<PlayingAreaComponent>(PlayingAreaComponent.StaticGroup);
			if(internalComponent == null) return;
			DrawTerrain(internalComponent.Terrain);

			// For debugging purposes only.
			//DrawTerrainQuadtree(internalComponent.Terrain.QuadtreeRoot);

			// Draw the children of the target entity.
			foreach(IModelEntity child in targetEntity.Children)
			{
				EntityRenderingComponent renderer = child.GetComponent<EntityRenderingComponent>(EntityRenderingComponent.StaticGroup);
				if(renderer != null)
				{
					m_entityEffect.World = m_matWorld;
					m_entityEffect.View = m_matView;
					m_entityEffect.Projection = m_matProjection;
					renderer.Draw(m_entityEffect, 1f);
				}
			}
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
		/// Draws the bounding boxes of the various nodes in a terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="root">The root node of the terrain quadtree to draw.</param>
		private void DrawTerrainQuadtree(QuadtreeNode root)
		{
			m_terrainQuadtreeEffect.World = m_matWorld;
			m_terrainQuadtreeEffect.View = m_matView;
			m_terrainQuadtreeEffect.Projection = m_matProjection;
			m_terrainQuadtreeEffect.VertexColorEnabled = true;

			DrawTerrainQuadtreeSub(root);
		}

		/// <summary>
		/// Draws the bounding boxes of the various nodes in a subtree of the terrain quadtree (for debugging purposes).
		/// </summary>
		/// <param name="node">The root node of the subtree.</param>
		/// <param name="depth">The depth of the recursion.</param>
		private void DrawTerrainQuadtreeSub(QuadtreeNode node, int depth = 0)
		{
			if(node.Children != null)
			{
				// This node is a branch, so recurse on its children.
				foreach(QuadtreeNode child in node.Children)
				{
					DrawTerrainQuadtreeSub(child, depth + 1);
				}
			}

			// Draw the node's own bounding box.
			var colours = new Color[] { Color.Cyan, Color.Yellow, Color.Magenta };
			Renderer.DrawBoundingBox(node.Bounds, m_terrainQuadtreeEffect, colours[depth % colours.Length]);
		}

		/// <summary>
		/// Sets up the world, view and projection matrices ready for rendering.
		/// </summary>
		private void SetupMatrices()
		{
			Camera camera = Entity.GetComponent<PlayInteractionComponent>(PlayInteractionComponent.StaticGroup).Camera;

			m_matProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), (float)Entity.Viewport.Width / Entity.Viewport.Height, 0.1f, 1000.0f);
			m_matView = Matrix.CreateLookAt(camera.Position, camera.Position + camera.N, camera.V);
			m_matWorld = Matrix.Identity;
		}

		#endregion
	}
}
