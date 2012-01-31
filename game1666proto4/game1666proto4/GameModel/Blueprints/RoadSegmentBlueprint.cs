/***
 * game1666proto4: RoadSegmentBlueprint.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for building a road segment.
	/// </summary>
	sealed class RoadSegmentBlueprint : PlaceableEntityBlueprint
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
	}
}
