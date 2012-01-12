/***
 * game1666proto4: EntityDeletionTool.cs
 * Copyright 2012. All rights reserved.
 ***/

using game1666proto4.Common.Graphics;
using game1666proto4.GameModel;
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
		/// A name relevant to the type of tool being used.
		/// </summary>
		public string Name { get { return "Delete"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new entity deletion tool.
		/// </summary>
		/// <param name="playingArea">The playing area from which to delete the entity.</param>
		public EntityDeletionTool(IPlayingArea playingArea)
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
			// Find the point we're hovering over on the near clipping plane.
			Vector3 near = viewport.Unproject(new Vector3(state.X, state.Y, 0), matProjection, matView, matWorld);

			// Find the point we're hovering over on the far clipping plane.
			Vector3 far = viewport.Unproject(new Vector3(state.X, state.Y, 1), matProjection, matView, matWorld);

			// Find the ray (in world space) between them.
			Vector3 dir = Vector3.Normalize(far - near);
			var ray = new Ray(near, dir);

			// TEMPORARY: Switch to using bounding boxes instead.
			Entity = null;
			foreach(IPlaceableEntity entity in m_playingArea.Children)
			{
				Model model = Renderer.Content.Load<Model>("Models/" + entity.Blueprint.Model);
				foreach(ModelMesh mesh in model.Meshes)
				{
					var boundingSphere = mesh.BoundingSphere.Transform(Matrix.CreateTranslation(new Vector3(entity.Position.X + 0.5f, entity.Position.Y + 0.5f, entity.Altitude)));
					if(ray.Intersects(boundingSphere) != null)
					{
						Entity = entity;
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
				// TEMPORARY: Entities should be removed over time, not instantaneously.
				m_playingArea.DeletePlaceableEntity(Entity);
			}
			return this;
		}

		#endregion
	}
}
