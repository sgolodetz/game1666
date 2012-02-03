/***
 * game1666proto4: MobileEntityBlueprint.cs
 * Copyright 2012. All rights reserved.
 ***/

using System.Xml.Linq;

namespace game1666proto4.GameModel.Blueprints
{
	/// <summary>
	/// An instance of this class represents a blueprint for constructing a mobile entity.
	/// </summary>
	abstract class MobileEntityBlueprint : Blueprint
	{
		//#################### PROPERTIES ####################
		#region

		// TODO: Things like the animation speed.

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a mobile entity blueprint from its XML representation.
		/// </summary>
		/// <param name="blueprintElt">The root element of the blueprint's XML representation.</param>
		public MobileEntityBlueprint(XElement blueprintElt)
		:	base(blueprintElt)
		{}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Adds an entity to the blueprint based on its dynamic type.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void AddDynamicEntity(dynamic entity)
		{
			// TODO
		}

		#endregion
	}
}
