/***
 * game1666: PlayInteractionComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameUI.Entities.Base;
using game1666.GameUI.Entities.Components.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace game1666.GameUI.Entities.Components.Play
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
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlayInteraction"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a play interaction component.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlayInteractionComponent(XElement componentElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
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
			Camera camera = Entity.GetComponent<PlayStateComponent>(PlayStateComponent.StaticGroup).Camera;
			KeyboardState keyState = Keyboard.GetState();
			if(keyState.IsKeyDown(Keys.W))		camera.MoveN(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.S))		camera.MoveN(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.A))		camera.MoveU(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.D))		camera.MoveU(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.Q))		camera.MoveV(keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.E))		camera.MoveV(-keyboardLinearRate);
			if(keyState.IsKeyDown(Keys.Left))	camera.Rotate(Vector3.UnitZ, keyboardAngularRateH);
			if(keyState.IsKeyDown(Keys.Right))	camera.Rotate(Vector3.UnitZ, -keyboardAngularRateH);
			if(keyState.IsKeyDown(Keys.Up))		camera.Rotate(camera.U, keyboardAngularRateV);
			if(keyState.IsKeyDown(Keys.Down))	camera.Rotate(camera.U, -keyboardAngularRateV);

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
					camera.Rotate(Vector3.UnitZ, xOffset * mouseAngularScalingFactor);
				}

				if(Math.Abs(yOffset) > mouseInactiveHalfHeight)
				{
					camera.Rotate(camera.U, yOffset * mouseAngularScalingFactor);
				}
			}
		}

		#endregion
	}
}
