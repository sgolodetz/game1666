/***
 * game1666: PlaceableComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Maths;
using game1666.Common.Messaging;
using game1666.Common.UI;
using game1666.GameModel.Blueprints;
using game1666.GameModel.Entities.Components.Context;
using game1666.GameModel.Entities.Components.Internal;
using game1666.GameModel.Entities.Messages;
using game1666.GameModel.Entities.Navigation;
using game1666.GameModel.Entities.PlacementStrategies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components.External
{
	/// <summary>
	/// An instance of this class makes its containing entity placeable on a terrain.
	/// </summary>
	class PlaceableComponent : ExternalComponent
	{
		//#################### ENUMERATIONS ####################
		#region

		/// <summary>
		/// The various possible states of a placeable component.
		/// </summary>
		private enum PlaceableComponentState
		{
			IN_CONSTRUCTION,	// the entity containing the component is in the process of being constructed
			IN_DESTRUCTION,		// the entity containing the component is in the process of being destructed
			OPERATING			// the entity containing the component is operating normally
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The footprint of the entity, suitably rotated to take account of its orientation.
		/// </summary>
		private readonly Footprint m_footprint;

		#endregion

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
		public PlaceableBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The amount of construction done (in comparison to the time required to construct the entity).
		/// </summary>
		private int ConstructionDone
		{
			get { return Properties["ConstructionDone"]; }
			set { Properties["ConstructionDone"] = Math.Max(Math.Min(value, Blueprint.TimeToConstruct), 0); }
		}

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		public bool Destructible
		{
			get
			{
				dynamic destructible;
				return Properties.TryGetValue("Destructible", out destructible) ? destructible : true;
			}
		}

		/// <summary>
		/// The entrances to the entity.
		/// </summary>
		public IEnumerable<Vector2i> Entrances
		{
			get
			{
				Vector2i offset = Position - m_footprint.Hotspot;
				return m_footprint.Entrances.Select(e => e + offset);
			}
		}

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Placement"; } }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		public Orientation4 Orientation { get { return Properties["Orientation"]; } }

		/// <summary>
		/// The completeness percentage of the entity.
		/// </summary>
		public int PercentComplete
		{
			get { return Blueprint.TimeToConstruct != 0 ? ConstructionDone * 100 / Blueprint.TimeToConstruct : 100; }
		}

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		public Vector2i Position { get { return Properties["Position"]; } }

		/// <summary>
		/// The state of the component.
		/// </summary>
		private PlaceableComponentState State
		{
			get { return Enum.Parse(typeof(PlaceableComponentState), Properties["State"]); }
			set { Properties["State"] = value.ToString(); }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placement component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public PlaceableComponent(XElement componentElt)
		:	base(componentElt)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);

			// Determine the entity's (suitably rotated) footprint.
			m_footprint = Blueprint.Footprint.Rotated((int)Orientation);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Called just after the entity containing this component is added as the child of another.
		/// </summary>
		public override void AfterAdd()
		{
			// Look up the playing area on which the entity containing this component resides.
			PlayingAreaComponent playingArea = Entity.Parent.GetComponent<PlayingAreaComponent>(PlayingAreaComponent.StaticGroup);

			// Determine the entity's altitude on the playing area's terrain.
			Altitude = playingArea.Terrain.DetermineAverageAltitude(Position);

			// Mark the space occupied by the entity on the navigation map.
			playingArea.NavigationMap.MarkOccupied
			(
				Blueprint.PlacementStrategy.Place
				(
					playingArea.Terrain,
					Blueprint.Footprint,
					Position,
					Orientation
				),
				Entity
			);

			// Register a message rule that causes the parent of the entity containing this component
			// to remove it as a child if the entity posts an entity destruction message.
			var messageSystem = Entity.GetRootEntity().GetComponent<ContextComponent>(ContextComponent.StaticGroup).MessageSystem;
			messageSystem.RegisterRule
			(
				new MessageRule<EntityDestructionMessage>
				{
					Action = new Action<EntityDestructionMessage>(msg => Entity.Parent.RemoveChild(Entity)),
					Entities = new List<dynamic> { Entity, Entity.Parent },
					Filter = MessageFilterFactory.TypedFromSource<EntityDestructionMessage>(Entity),
					Key = "removechild:" + Entity.GetAbsolutePath()
				}
			);
		}

		/// <summary>
		/// Called just before the entity containing this component is removed as the child of another.
		/// </summary>
		public override void BeforeRemove()
		{
			// Look up the playing area on which the entity containing this component resides.
			PlayingAreaComponent playingArea = Entity.Parent.GetComponent<PlayingAreaComponent>(PlayingAreaComponent.StaticGroup);

			// Clear the space occupied by the entity on the navigation map.
			playingArea.NavigationMap.MarkOccupied
			(
				Blueprint.PlacementStrategy.Place
				(
					playingArea.Terrain,
					Blueprint.Footprint,
					Position,
					Orientation
				),
				null
			);

			// Unregister the remove child message rule added in AfterAdd().
			var messageSystem = Entity.GetRootEntity().GetComponent<ContextComponent>(ContextComponent.StaticGroup).MessageSystem;
			messageSystem.UnregisterRule("removechild:" + Entity.GetAbsolutePath());
		}

		/// <summary>
		/// Draws the placeable entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		public override void Draw(BasicEffect effect, float alpha)
		{
			// Determine the model name and orientation to use (see the description on the method).
			EntityNavigationMap navigationMap = Entity.Parent.GetComponent<PlayingAreaComponent>(PlayingAreaComponent.StaticGroup).NavigationMap;
			if(navigationMap == null) return;

			Tuple<string,Orientation4> result = DetermineModelAndOrientation(Blueprint.Model, Orientation, navigationMap);
			string modelName = result.Item1;
			Orientation4 orientation = result.Item2;

			// Load the model.
			Model model = Renderer.Content.Load<Model>("Models/" + modelName);

			// Move the model to the correct position.
			Matrix matWorld = Matrix.CreateTranslation(Position.X + 0.5f, Position.Y + 0.5f, Altitude);

			// If the entity has a non-default orientation, rotate the model appropriately.
			if(orientation != Orientation4.XPOS)
			{
				float angle = Convert.ToInt32(orientation) * MathHelper.PiOver2;
				Matrix matRot = Matrix.CreateRotationZ(angle);
				matWorld = Matrix.Multiply(matRot, matWorld);
			}

			// If the entity is being constructed or destructed, scale the model based on the current state of completion.
			if(State == PlaceableComponentState.IN_CONSTRUCTION || State == PlaceableComponentState.IN_DESTRUCTION)
			{
				Matrix matScale = Matrix.CreateScale(1, 1, PercentComplete / 100f);
				matWorld = Matrix.Multiply(matScale, matWorld);
			}

			// Render the model.
			Renderer.DrawModel(model, matWorld, effect.View, effect.Projection, alpha);
		}

		/// <summary>
		/// Updates the component based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			switch(State)
			{
				case PlaceableComponentState.IN_CONSTRUCTION:
				{
					ConstructionDone += gameTime.ElapsedGameTime.Milliseconds;
					if(ConstructionDone == Blueprint.TimeToConstruct)
					{
						State = PlaceableComponentState.OPERATING;
					}
					break;
				}
				case PlaceableComponentState.IN_DESTRUCTION:
				{
					ConstructionDone -= gameTime.ElapsedGameTime.Milliseconds;
					if(ConstructionDone == 0)
					{
						var destructionManager = Entity.GetRootEntity().GetComponent<ContextComponent>(ContextComponent.StaticGroup).DestructionManager;
						destructionManager.QueueForDestruction(Entity);
					}
					break;
				}
				default:	// PlacementComponentState.OPERATING
				{
					break;
				}
			}
		}

		#endregion

		//#################### PROTECTED METHODS ####################
		#region

		/// <summary>
		/// Determines the actual model and orientation to use when drawing the placeable entity.
		/// This is a hook so that we can override the default behaviour when drawing things like
		/// road segments. For most entities, we just use this default implementation, which returns
		/// the passed in model name and orientation.
		/// </summary>
		/// <param name="modelName">The initial model name, as specified by the blueprint.</param>
		/// <param name="orientation">The initial orientation.</param>
		/// <param name="navigationMap">The navigation map associated with the terrain on which the entity sits.</param>
		/// <returns>The actual model and orientation to use.</returns>
		protected virtual Tuple<string,Orientation4> DetermineModelAndOrientation(string modelName, Orientation4 orientation, EntityNavigationMap navigationMap)
		{
			return Tuple.Create(modelName, orientation);
		}

		#endregion
	}
}
