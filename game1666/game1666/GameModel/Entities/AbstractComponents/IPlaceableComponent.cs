/***
 * game1666: IPlaceableComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using game1666.Common.Maths;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;
using game1666.GameModel.Navigation;

namespace game1666.GameModel.Entities.AbstractComponents
{
	/// <summary>
	/// The various possible states of a placeable component.
	/// </summary>
	enum PlaceableComponentState
	{
		IN_CONSTRUCTION,	// the entity containing the component is in the process of being constructed
		IN_DESTRUCTION,		// the entity containing the component is in the process of being destructed
		OPERATING			// the entity containing the component is operating normally
	}

	/// <summary>
	/// An instance of a class implementing this interface makes its containing entity placeable on a terrain.
	/// </summary>
	interface IPlaceableComponent : IExternalComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The altitude of the base of the entity.
		/// </summary>
		float Altitude { get; set; }

		/// <summary>
		/// The blueprint for the entity.
		/// </summary>
		PlaceableBlueprint Blueprint { get; }

		/// <summary>
		/// Whether or not the entity can be destroyed.
		/// </summary>
		bool Destructible { get; }

		/// <summary>
		/// The entrances to the entity.
		/// </summary>
		IEnumerable<Vector2i> Entrances { get; }

		/// <summary>
		/// The 2D axis-aligned orientation of the entity.
		/// </summary>
		Orientation4 Orientation { get; }

		/// <summary>
		/// The completeness percentage of the entity.
		/// </summary>
		int PercentComplete { get; }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the entity's hotspot.
		/// </summary>
		Vector2i Position { get; }

		/// <summary>
		/// The state of the component.
		/// </summary>
		PlaceableComponentState State { get; }

		#endregion

		//#################### PUBLIC METHODS ####################
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
		Tuple<string,Orientation4> DetermineModelAndOrientation(string modelName, Orientation4 orientation, INavigationMap<IModelEntity> navigationMap);

		/// <summary>
		/// Sets the state of the component to IN_DESTRUCTION, which will
		/// ultimately lead to the destruction of the containing entity.
		/// </summary>
		void InitiateDestruction();

		/// <summary>
		/// Checks whether or not the entity containing this component can be validly
		/// placed on the terrain of the specified playing area entity, bearing in
		/// mind its footprint, position and orientation.
		/// </summary>
		/// <param name="playingAreaEntity">The playing area entity.</param>
		/// <returns>true, if the entity containing this component can be validly placed, or false otherwise.</returns>
		bool IsValidlyPlaced(IModelEntity playingAreaEntity);

		#endregion
	}
}
