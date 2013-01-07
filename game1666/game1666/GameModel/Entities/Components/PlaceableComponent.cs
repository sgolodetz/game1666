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
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Entities.Extensions;
using game1666.GameModel.Entities.Interfaces.Components;
using game1666.GameModel.Entities.Messages;
using game1666.GameModel.Entities.PlacementStrategies;
using game1666.GameModel.Navigation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class makes its containing entity placeable on a terrain.
	/// </summary>
	class PlaceableComponent : ModelEntityComponent, IPlaceableComponent
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The entrances to the entity.
		/// </summary>
		private List<Vector2i> m_entrances;

		/// <summary>
		/// The footprint of the entity, suitably rotated to take account of its orientation.
		/// </summary>
		private Footprint m_footprint;

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
		/// The blueprint for the component.
		/// </summary>
		public PlaceableBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The amount of construction done, in the range [0, Blueprint.TimeToConstruct].
		/// </summary>
		public int ConstructionDone
		{
			get { return Properties["ConstructionDone"]; }
			set { Properties["ConstructionDone"] = Math.Max(Math.Min(value, Blueprint.TimeToConstruct), 0); }
		}

		/// <summary>
		/// Whether or not the entity can be destroyed (true by default).
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
		public List<Vector2i> Entrances { get { return m_entrances; } }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.EXTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Placeable"; } }

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
		public PlaceableComponentState State
		{
			get	{ return (PlaceableComponentState)Enum.Parse(typeof(PlaceableComponentState), Properties["State"]); }
			set	{ Properties["State"] = value.ToString(); }
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a placeable component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public PlaceableComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);

			// Determine the entity's (suitably rotated) footprint.
			m_footprint = Blueprint.Footprint.Rotated((int)Orientation);

			// Determine the entity's entrances.
			Vector2i offset = Position - m_footprint.Hotspot;
			m_entrances = m_footprint.Entrances.Select(e => e + offset).ToList();
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
			var playingArea = Entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

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
			this.MessageSystem().RegisterRule
			(
				new MessageRule<ModelEntityDestructionMessage>
				{
					Action = new Action<ModelEntityDestructionMessage>(msg => Entity.Parent.RemoveChild(Entity)),
					Entities = new List<dynamic> { Entity, Entity.Parent },
					Filter = MessageFilterFactory.TypedFromSource<ModelEntityDestructionMessage>(Entity),
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
			var playingArea = Entity.Parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);

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
			this.MessageSystem().UnregisterRule("removechild:" + Entity.GetAbsolutePath());
		}

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
		public virtual Tuple<string,Orientation4> DetermineModelAndOrientation(string modelName, Orientation4 orientation, INavigationMap<ModelEntity> navigationMap)
		{
			return Tuple.Create(modelName, orientation);
		}

		/// <summary>
		/// Draws the placeable entity of which this component is a part.
		/// </summary>
		/// <param name="effect">The basic effect to use when drawing.</param>
		/// <param name="alpha">The alpha value to use when drawing.</param>
		/// <param name="parent">The parent of the entity (used when rendering
		/// entities that have not yet been attached to their parent).</param>
		public void Draw(BasicEffect effect, float alpha, ModelEntity parent = null)
		{
			// We need the navigation map from the entity's parent here because it affects how
			// we draw things like road segments (these are rendered differently depending on
			// what entities there are in the grid squares next to them). However, it's not
			// always possible to just look up the parent in the entity tree - in particular,
			// entities which are currently being placed are not yet attached to the tree. For
			// that reason, a parent parameter to Draw is provided that allows the caller to
			// explicitly specify the correct parent. If they don't supply a parent, we assume
			// it's because the entity's attached to the tree and try and look up the parent
			// there. If that fails, we early out.
			parent = parent ?? Entity.Parent;
			if(parent == null) return;

			// Determine the model name and orientation to use (see the description on the DetermineModelAndOrientation method).
			INavigationMap<ModelEntity> navigationMap = parent.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL).NavigationMap;
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
		/// Sets the state of the component to IN_DESTRUCTION, which will
		/// ultimately lead to the destruction of the containing entity.
		/// </summary>
		public void InitiateDestruction()
		{
			State = PlaceableComponentState.IN_DESTRUCTION;
		}

		/// <summary>
		/// Checks whether or not the entity containing this component can be validly
		/// placed on the terrain of the specified playing area entity, bearing in
		/// mind its footprint, position and orientation.
		/// </summary>
		/// <param name="playingAreaEntity">The playing area entity.</param>
		/// <returns>true, if the entity containing this component can be validly placed, or false otherwise.</returns>
		public bool IsValidlyPlaced(ModelEntity playingAreaEntity)
		{
			var playingAreaComponent = playingAreaEntity.GetComponent<IPlayingAreaComponent>(ModelEntityComponentGroups.INTERNAL);
			if(playingAreaComponent == null) return false;

			// Step 1:	Check that the entity occupies one or more grid squares, and that all the grid squares it does occupy are empty.
			IEnumerable<Vector2i> gridSquares = Blueprint.PlacementStrategy.Place
			(
				playingAreaComponent.Terrain,
				Blueprint.Footprint,
				Position,
				Orientation
			);

			if(gridSquares == null || !gridSquares.Any() || playingAreaComponent.NavigationMap.AreOccupied(gridSquares))
			{
				return false;
			}

			// Step 2:	Check that there are currently no mobile entities in the grid squares that the entity would occupy.
			//			Note that this isn't an especially efficient way of going about this, but it will do for now.
			//			A better approach would involve keeping track of which mobile entities are in which grid squares,
			//			and then checking per-grid square rather than per-entity.
			var gridSquareSet = new HashSet<Vector2i>(gridSquares);
			foreach(ModelEntity child in playingAreaEntity.Children)
			{
				var mobileComponent = child.GetComponent<IMobileComponent>(ModelEntityComponentGroups.EXTERNAL);
				if(mobileComponent == null) continue;

				if(gridSquareSet.Contains(mobileComponent.Position.ToVector2i()))
				{
					return false;
				}
			}

			// If we didn't find any problems, then the entity is validly placed.
			return true;
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
						this.EntityDestructionManager().QueueForDestruction(Entity);
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
	}
}
