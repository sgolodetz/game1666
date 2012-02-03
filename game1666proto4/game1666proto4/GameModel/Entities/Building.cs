/***
 * game1666proto4: Building.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.GameModel.FSMs;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : PlaceableEntity, IUpdateableEntity
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The placement strategy for the building.
		/// </summary>
		public override IPlacementStrategy PlacementStrategy { get { return new PlacementStrategyRequireFlatGround(); } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a building directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the building.</param>
		/// <param name="initialStateID">The initial state of the building.</param>
		public Building(IDictionary<string,dynamic> properties, PlaceableEntityStateID initialStateID)
		:	base(properties, initialStateID)
		{
			SetName();
		}

		/// <summary>
		/// Constructs a building from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the building's XML representation.</param>
		public Building(XElement entityElt)
		:	base(entityElt)
		{
			SetName();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the building based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			AddEntity(entity);
		}

		/// <summary>
		/// Updates the building based on elapsed time and user input.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Update(GameTime gameTime)
		{
			FSM.Update(gameTime);
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Makes sure that the building has an appropriate name.
		/// </summary>
		private void SetName()
		{
			Properties["Name"] = "building:" + Guid.NewGuid().ToString();
		}

		#endregion
	}
}
