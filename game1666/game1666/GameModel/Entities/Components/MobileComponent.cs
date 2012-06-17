/***
 * game1666: MobileComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.Common.UI;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Interfaces.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class allows an entity to move around on a terrain.
	/// </summary>
	sealed class MobileComponent : ModelEntityComponent, IMobileComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		public float Altitude
		{
			get { return Properties["Altitude"]; }
			set { Properties["Altitude"] = value; }
		}

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		public MobileBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.EXTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Mobile"; } }

		/// <summary>
		/// The orientation of the entity (as an anti-clockwise angle in radians, where 0 means facing right).
		/// </summary>
		public float Orientation
		{
			get	{ return Properties["Orientation"]; }
			set	{ Properties["Orientation"] = value; }
		}

		/// <summary>
		/// The position of the entity (relative to the origin of the containing entity).
		/// </summary>
		public Vector2 Position
		{
			get	{ return Properties["Position"]; }
			set	{ Properties["Position"] = value; }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a mobile component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public MobileComponent(XElement componentElt)
		:	base(componentElt)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after the entity containing this component is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			// If the entity to which this component's entity is being added has a playing area,
			// recalculate the entity's altitude on that playing area's terrain.
			var playingArea = Entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			if(playingArea != null)
			{
				Altitude = playingArea.Terrain.DetermineAltitude(Position);
			}
		}

		/// <summary>
		/// Draws the mobile entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		/// <param name="parent">The parent of the entity (used when rendering
		/// entities that have not yet been attached to their parent).</param>
		public void Draw(BasicEffect effect, float alpha, ModelEntity parent = null)
		{
			// TEMPORARY
			var pos = new Vector3(Position, Altitude);
			var bounds = new BoundingBox(pos - new Vector3(0.1f, 0.1f, 0f), pos + new Vector3(0.1f, 0.1f, 0.2f));

			// Rotate the entity as necessary based on its orientation.
			Matrix matRot = Matrix.CreateTranslation(pos);
			matRot = Matrix.Multiply(Matrix.CreateRotationZ(Orientation), matRot);
			matRot = Matrix.Multiply(Matrix.CreateTranslation(-pos), matRot);

			effect.World = Matrix.Multiply(matRot, effect.World);
			effect.VertexColorEnabled = true;

			Renderer.DrawBoundingBox(bounds, effect, Color.Red);
		}

		#endregion
	}
}
