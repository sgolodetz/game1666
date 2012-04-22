/***
 * game1666: PlayInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using game1666.Common.UI;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameUI.Entities.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components.Interaction
{
	/// <summary>
	/// An instance of this class provides interaction behaviour to a play viewer that
	/// shows the contents of a playing area (such as the world or a settlement).
	/// </summary>
	sealed class PlayInteractionComponent : InteractionComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The 3D camera specifying the position of the viewer.
		/// </summary>
		public Camera Camera { get; private set; }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayInteraction"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a play interaction component.
		/// </summary>
		public PlayInteractionComponent()
		{
			Camera = new Camera(new Vector3(2, -5, 5), new Vector3(0, 2, -1), Vector3.UnitZ);
		}

		#endregion

		//#################### PUBLIC ABSTRACT METHODS ####################
		#region

		/// <summary>
		/// Handles mouse moved events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMouseMoved(MouseState state)
		{
			// TODO
		}

		/// <summary>
		/// Handles mouse pressed events.
		/// </summary>
		/// <param name="state">The mouse state at the point at which the mouse check was made.</param>
		public override void OnMousePressed(MouseState state)
		{
			// TODO
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// Look up the playing area component of the target of the game view containing the play viewer.
			var playingArea = UIEntityComponentUtil.GetTargetComponent<PlayingAreaComponent>(Entity.Parent, PlayingAreaComponent.StaticGroup);
			if(playingArea == null) return;

			// Determine the linear, horizontal angular, and vertical angular rates for keyboard-based movement.
			float[,] heightmap = playingArea.Terrain.Heightmap;
			float scalingFactor = Math.Max(heightmap.GetLength(0), heightmap.GetLength(1));
			float keyboardLinearRate = 0.0005f * scalingFactor * gameTime.ElapsedGameTime.Milliseconds;
			float keyboardAngularRateH = 0.002f * gameTime.ElapsedGameTime.Milliseconds;	// in radians
			float keyboardAngularRateV = 0.0015f * gameTime.ElapsedGameTime.Milliseconds;	// in radians

			// Alter the camera based on keyboard input.
			KeyboardState keyState = Keyboard.GetState();
			if(keyState.IsKeyDown(Keys.W))		Camera.MoveN(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.S))		Camera.MoveN(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.A))		Camera.MoveU(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.D))		Camera.MoveU(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.Q))		Camera.MoveV(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.E))		Camera.MoveV(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.Left))	Camera.Rotate(Vector3.UnitZ, keyboardAngularRateH);
			if(keyState.IsKeyDown(Keys.Right))	Camera.Rotate(Vector3.UnitZ, -keyboardAngularRateH);
			if(keyState.IsKeyDown(Keys.Up))		Camera.Rotate(Camera.U, keyboardAngularRateV);
			if(keyState.IsKeyDown(Keys.Down))	Camera.Rotate(Camera.U, -keyboardAngularRateV);

			// Note: Mouse-based input is only active when the left shift key is pressed - it would be annoying otherwise.
			if(keyState.IsKeyDown(Keys.LeftShift))
			{
				// Determine the scaling factor that controls the angular rate for mouse-based movement.
				float mouseAngularScalingFactor = 0.000005f * gameTime.ElapsedGameTime.Milliseconds;

				// Determine (half) the size of the region in the centre of the screen where mouse-based movement is inactive.
				int mouseInactiveHalfWidth = Entity.Viewport.Width / 8;
				int mouseInactiveHalfHeight = Entity.Viewport.Height / 8;

				// Provided the cursor is outside the inactive region, alter the camera based on mouse input.
				MouseState mouseState = Mouse.GetState();
				float xOffset = Entity.Viewport.X + Entity.Viewport.Width * 0.5f - mouseState.X;
				float yOffset = mouseState.Y - (Entity.Viewport.Y + Entity.Viewport.Height * 0.5f);

				if(Math.Abs(xOffset) > mouseInactiveHalfWidth)
				{
					Camera.Rotate(Vector3.UnitZ, xOffset * mouseAngularScalingFactor);
				}

				if(Math.Abs(yOffset) > mouseInactiveHalfHeight)
				{
					Camera.Rotate(Camera.U, yOffset * mouseAngularScalingFactor);
				}
			}
		}

		#endregion
	}
}
