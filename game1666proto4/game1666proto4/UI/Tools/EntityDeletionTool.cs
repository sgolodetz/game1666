/***
 * game1666proto4: EntityDeletionTool.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Linq;
using game1666proto4.Common.Graphics;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Entities;
using game1666proto4.GameModel.FSMs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace game1666proto4.UI.Tools
{
	/// <summary>
	/// An instance of this class can be used to delete entities from a playing area.
	/// </summary>
	sealed class EntityDeletionTool : ITool
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The playing area in which to place the entity.
		/// </summary>
		private readonly IPlayingArea m_playingArea;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The entity currently being targeted for deletion by the user (if any).
		/// </summary>
		public IPlaceableEntity Entity { get; private set; }

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
		public EntityDeletionTool(string name, IPlayingArea playingArea)
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
			Tuple<Vector2i,float> gridSquareAndDistance = m_playingArea.Terrain.PickGridSquare(ray);
			float nearestHitDistance = gridSquareAndDistance != null ? gridSquareAndDistance.Item2 : float.MaxValue;

			// Find the nearest entity (if any) that is (a) not occluded by the terrain, (b) hit by the ray and (c) destructible,
			// and mark it for deletion.
			Entity = null;
			foreach(IPlaceableEntity entity in m_playingArea.Children.Where(c => c.Destructible))
			{
				string modelName = EntityUtil.DetermineModelNameAndOrientation((dynamic)entity, m_playingArea.OccupancyMap).Item1;
				Model model = Renderer.Content.Load<Model>("Models/" + modelName);
				foreach(ModelMesh mesh in model.Meshes)
				{
					var boundingSphere = mesh.BoundingSphere.Transform(Matrix.CreateTranslation(new Vector3(entity.Position.X + 0.5f, entity.Position.Y + 0.5f, entity.Altitude)));
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
				Entity.FSM.ForceState(PlaceableEntityStateID.IN_DESTRUCTION);
			}
			return this;
		}

		#endregion
	}
}
