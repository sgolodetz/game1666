/***
 * game1666: EntityDeletionTool.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.Maths;
using game1666.Common.UI;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Components.External;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Terrains;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Tools
{
	/// <summary>
	/// An instance of this class can be used to delete entities from a playing area.
	/// </summary>
	sealed class EntityDeletionTool : ITool
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The playing area from which to delete the entity.
		/// </summary>
		private readonly IModelEntity m_playingArea;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity currently being targeted for deletion by the user (if any).
		/// </summary>
		public IModelEntity Entity { get; private set; }

		/// <summary>
		/// The name of the tool.
		/// </summary>
		public string Name { get { return "Delete:Delete"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity deletion tool.
		/// </summary>
		/// <param name="name">The name of the tool (dummy parameter).</param>
		/// <param name="playingArea">The playing area from which to delete the entity.</param>
		public EntityDeletionTool(string name, IModelEntity playingArea)
		{
			m_playingArea = playingArea;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		/// <param name="viewport">The viewport of the viewer being used to interact with the playing area.</param>
		/// <param name="matProjection">The current projection matrix in that viewer.</param>
		/// <param name="matView">The current view matrix in that viewer.</param>
		/// <param name="matWorld">The current world matrix in that viewer.</param>
		public void OnMouseMoved(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			// Determine the 3D world space ray corresponding to the location of the user's mouse in the viewport.
			var ray = ToolUtil.DetermineMouseRay(state, viewport, matProjection, matView, matWorld);

			// Find the distance at which the ray hits the terrain, if it does so.
			PlayingAreaComponent playingAreaComponent = m_playingArea.GetComponent(PlayingAreaComponent.StaticGroup);
			Tuple<Vector2i,float> gridSquareAndDistance = playingAreaComponent.Terrain.PickGridSquare(ray);
			float nearestHitDistance = gridSquareAndDistance != null ? gridSquareAndDistance.Item2 : float.MaxValue;

			// Find the nearest placeable entity (if any) that is (a) not occluded by the terrain, (b) hit by the ray
			// and (c) destructible, and mark it for deletion.
			Entity = null;
			foreach(IModelEntity entity in m_playingArea.Children)
			{
				PlaceableComponent placeableComponent = entity.GetComponent(PlaceableComponent.StaticGroup);
				if(placeableComponent == null || !placeableComponent.Destructible) continue;

				// Load the entity's model.
				string modelName = placeableComponent.DetermineModelAndOrientation(placeableComponent.Blueprint.Model, placeableComponent.Orientation, playingAreaComponent.NavigationMap).Item1;
				Model model = Renderer.Content.Load<Model>("Models/" + modelName);

				// Create a matrix to translate the bounding spheres in the model to the correct
				// position in world space.
				Matrix offsetMatrix = Matrix.CreateTranslation(new Vector3(placeableComponent.Position.X + 0.5f, placeableComponent.Position.Y + 0.5f, placeableComponent.Altitude));

				// Run through the meshes in the model, translate their bounding spheres based
				// on the actual position of the entity in world space and test the ray against
				// the translated spheres. If the ray hits one of the spheres, and the hit is
				// the nearest one we've seen so far, update the entity to be deleted accordingly.
				foreach(ModelMesh mesh in model.Meshes)
				{
					var boundingSphere = mesh.BoundingSphere.Transform(offsetMatrix);
					float? hitDistance = ray.Intersects(boundingSphere);
					if(hitDistance != null && hitDistance.Value < nearestHitDistance)
					{
						Entity = entity;
						nearestHitDistance = hitDistance.Value;
					}
				}
			}
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		/// <param name="viewport">The viewport of the viewer being used to interact with the playing area.</param>
		/// <param name="matProjection">The current projection matrix in that viewer.</param>
		/// <param name="matView">The current view matrix in that viewer.</param>
		/// <param name="matWorld">The current world matrix in that viewer.</param>
		/// <returns>The tool that should be active after the mouse press (generally this or null).</returns>
		public ITool OnMousePressed(MouseState state, Viewport viewport, Matrix matProjection, Matrix matView, Matrix matWorld)
		{
			if(state.LeftButton == ButtonState.Pressed && Entity != null)
			{
				PlaceableComponent placeableComponent = Entity.GetComponent(PlaceableComponent.StaticGroup);
				placeableComponent.InitiateDestruction();
			}
			return this;
		}

		#endregion
	}
}
