/***
 * game1666proto4: Building.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a building.
	/// </summary>
	abstract class Building : Entity, IBuildableModelEntity
	{
		//#################### PRIVATE VARIABLES ####################
		#region

		/// <summary>
		/// The blueprint for the building.
		/// </summary>
		private readonly Blueprint m_blueprint;

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		private readonly Vector2i m_position;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the building.
		/// </summary>
		public Blueprint Blueprint { get { return m_blueprint; } }

		/// <summary>
		/// The position (relative to the origin of the containing entity) of the building's hotspot.
		/// </summary>
		public Vector2i Position { get { return m_position; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a building from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root node of the building's XML representation.</param>
		public Building(XElement entityElt)
		:	base(entityElt)
		{
			m_blueprint = SceneGraph.GetEntityByPath("blueprints/" + Properties["Blueprint"]);
			// TODO: Get the position.
		}

		#endregion
	}
}
