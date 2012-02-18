/***
 * game1666proto4: MovementStrategyGoToPosition.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666proto4.Common.Entities;
using game1666proto4.Common.Maths;
using game1666proto4.GameModel.Blueprints;
using game1666proto4.GameModel.Navigation;
using Microsoft.Xna.Framework;

namespace game1666proto4.GameModel.Entities
{
	/// <summary>
	/// An instance of this class represents a movement strategy that causes a mobile entity to head towards a specific position.
	/// </summary>
	sealed class MovementStrategyGoToPosition : IMovementStrategy
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The properties of the movement strategy.
		/// </summary>
		private readonly IDictionary<string,dynamic> m_properties;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The properties of the mobile entity.
		/// </summary>
		public IDictionary<string,dynamic> EntityProperties { private get; set; }

		/// <summary>
		/// The navigation map for the terrain on which the entity is moving.
		/// </summary>
		public EntityNavigationMap NavigationMap { private get; set; }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a 'go to position' movement strategy from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the strategy's XML representation.</param>
		public MovementStrategyGoToPosition(XElement entityElt)
		{
			m_properties = EntityPersister.LoadProperties(entityElt);
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Moves the entity based on the movement strategy and elapsed time.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public void Move(GameTime gameTime)
		{
			MobileEntityBlueprint blueprint = BlueprintManager.GetBlueprint(EntityProperties["Blueprint"]);

			Vector3 pos = EntityProperties["Position"];
			Vector2 offset = m_properties["TargetPosition"] - pos.XY();

			if(offset.Length() > Constants.EPSILON)
			{
				offset.Normalize();
				offset *= blueprint.MovementSpeed * gameTime.ElapsedGameTime.Milliseconds / 1000f;

				pos.X += offset.X;
				pos.Y += offset.Y;
				pos.Z = NavigationMap.Terrain.DetermineAltitude(pos.XY());

				EntityProperties["Position"] = pos;
			}
		}

		/// <summary>
		/// Saves the movement strategy to XML.
		/// </summary>
		/// <returns>An XML representation of the movement strategy.</returns>
		public XElement SaveToXML()
		{
			XElement entityElt = EntityPersister.ConstructEntityElement(GetType());
			EntityPersister.SaveProperties(entityElt, m_properties);
			return entityElt;
		}

		#endregion
	}
}
