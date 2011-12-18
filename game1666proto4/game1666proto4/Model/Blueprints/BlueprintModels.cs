/***
 * game1666proto4: BlueprintModels.cs
 * Copyright 2011. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4
{
	/// <summary>
	/// An instance of this class holds the model references for a given blueprint.
	/// </summary>
	sealed class BlueprintModels : Entity
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blueprint model set from its XML representation.
		/// </summary>
		/// <param name="entityElt">The root element of the blueprint model set's XML representation.</param>
		public BlueprintModels(XElement entityElt)
		:	base(entityElt)
		{}

		#endregion
	}
}
