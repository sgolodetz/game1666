/***
 * game1666proto4: WalkerBlueprint.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing a walker.
	/// </summary>
	sealed class WalkerBlueprint : MobileEntityBlueprint
	{
		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a walker blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public WalkerBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion
	}
}
