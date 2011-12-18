﻿/***
 * game1666proto4: RoadSegmentBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class represents a blueprint for building a road segment.
	/// </summary>
	sealed class RoadSegmentBlueprint : Blueprint
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a road segment blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		/// <returns>The road segment blueprint.</returns>
		public RoadSegmentBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddEntityDynamic(dynamic entity)
		{
			AddEntity(entity);
		}

		#endregion
	}
}
