/***
 * game1666: HomeComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Xml.Linq;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Blueprints;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class allows its containing entity to act as a home for entities.
	/// </summary>
	sealed class HomeComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The blueprint for the component.
		/// </summary>
		public HomeBlueprint Blueprint { get; private set; }

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.INTERNAL; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Home"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a home component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		public HomeComponent(XElement componentElt)
		:	base(componentElt)
		{
			Blueprint = BlueprintManager.GetBlueprint(Properties["Blueprint"]);
		}

		#endregion
	}
}
