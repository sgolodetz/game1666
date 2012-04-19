﻿/***
 * game1666: PlaceableEntityRenderingComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.UI;
using game1666.GameModel.Entities.Components.External;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components.Rendering
{
	/// <summary>
	/// An instance of a class deriving from this one provides rendering behaviour to a placeable entity.
	/// </summary>
	sealed class PlaceableEntityRenderingComponent : EntityRenderingComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "PlaceableEntityRendering"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placeable entity rendering component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlaceableEntityRenderingComponent(XElement componentElt)
		:	base(componentElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Draws the placeable entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		public override void Draw(BasicEffect effect, float alpha)
		{
			// Determine the model name and orientation to use - this is a hook so that we can override
			// the default behaviour when drawing things like road segments. For most entities, we use
			// the default implementation, which just returns the entity's model name and orientation.
			/*Tuple<string,Orientation4> result = EntityUtil.DetermineModelNameAndOrientation((dynamic)entity, m_playingArea.NavigationMap);
			string modelName = result.Item1;
			Orientation4 orientation = result.Item2;*/

			//@@@
			PlacementComponent placement = Entity.GetComponent<PlacementComponent>(PlacementComponent.StaticGroup);
			if(placement == null) return;
			string modelName = placement.Blueprint.Model;
			Orientation4 orientation = placement.Orientation;
			//@@@

			// Load the model.
			Model model = Renderer.Content.Load<Model>("Models/" + modelName);

			// Move the model to the correct position.
			Matrix matWorld = Matrix.CreateTranslation(placement.Position.X + 0.5f, placement.Position.Y + 0.5f, 0f/*placement.Altitude*/);

			// If the entity has a non-default orientation, rotate the model appropriately.
			if(orientation != Orientation4.XPOS)
			{
				float angle = Convert.ToInt32(orientation) * MathHelper.PiOver2;
				Matrix matRot = Matrix.CreateRotationZ(angle);
				matWorld = Matrix.Multiply(matRot, matWorld);
			}

			// If the entity is being constructed or destructed, scale the model based on the current state of completion.
			if(placement.State == PlacementComponentState.IN_CONSTRUCTION || placement.State == PlacementComponentState.IN_DESTRUCTION)
			{
				Matrix matScale = Matrix.CreateScale(1, 1, placement.PercentComplete / 100f);
				matWorld = Matrix.Multiply(matScale, matWorld);
			}

			// Render the model.
			Renderer.DrawModel(model, matWorld, effect.View, effect.Projection, alpha);
		}

		#endregion
	}
}
