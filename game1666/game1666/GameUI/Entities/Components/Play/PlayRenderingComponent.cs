/***
 * game1666: PlayRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components.External;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Terrains;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameUI.Entities.Components.Play
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
		/// Constructs a play rendering component.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayRenderingComponent(XElement componentElt)
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
			IModelEntity targetEntity = Entity.Parent.GetTarget();
			if(targetEntity == null) return;

			// Prepare for rendering.
			Renderer.GraphicsDevice.Viewport = Entity.Viewport;
			Renderer.Setup3D();

			// Sets the world, view and projection matrices based on the current state of the camera.
			PlayStateComponent stateComponent = Entity.GetComponent(PlayStateComponent.StaticGroup);
			stateComponent.SetMatricesFromCamera();

			// Draw the terrain.
			var internalComponent = targetEntity.GetComponent(PlayingAreaComponent.StaticGroup);
			if(internalComponent == null) return;
			DrawTerrain(internalComponent.Terrain);

			// For debugging purposes only.
			//DrawTerrainQuadtree(internalComponent.Terrain.QuadtreeRoot);

			// Draw the children of the target entity.
			foreach(IModelEntity child in targetEntity.Children)
			{
				ExternalComponent childExternalComponent = child.GetComponent(ExternalComponent.StaticGroup);
				if(childExternalComponent != null)
				{
					m_entityEffect.World = stateComponent.WorldMatrix;
					m_entityEffect.View = stateComponent.ViewMatrix;
					m_entityEffect.Projection = stateComponent.ProjectionMatrix;
					childExternalComponent.Draw(m_entityEffect, 1f);
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
			PlayStateComponent stateComponent = Entity.GetComponent(PlayStateComponent.StaticGroup);
			effect.Parameters["World"].SetValue(stateComponent.WorldMatrix);
			effect.Parameters["View"].SetValue(stateComponent.ViewMatrix);
			effect.Parameters["Projection"].SetValue(stateComponent.ProjectionMatrix);
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
			PlayStateComponent stateComponent = Entity.GetComponent(PlayStateComponent.StaticGroup);
			m_terrainQuadtreeEffect.World = stateComponent.WorldMatrix;
			m_terrainQuadtreeEffect.View = stateComponent.ViewMatrix;
			m_terrainQuadtreeEffect.Projection = stateComponent.ProjectionMatrix;
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

		#endregion
	}
}
