/***
 * game1666: ObjectivesComponent.cs
 * Copyright Stuart Golodetz, 2013. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using game1666.Common.Persistence;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Objectives;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this component manages a set of objectives that must be satisfied for
	/// its containing world to be considered complete.
	/// </summary>
	sealed class ObjectivesComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The objectives that must be satisfied for the world containing this component to be considered complete.
		/// </summary>
		public IEnumerable<Objective> Objectives { get; private set; }

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.OBJECTIVES; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Objectives"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs an objectives component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public ObjectivesComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{
			Objectives = ObjectPersister.LoadChildObjects<Objective>(componentElt).ToList();
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// TODO
		/// </summary>
		/// <returns></returns>
		public override XElement SaveToXML()
		{
			// TODO
			return null;
		}

		#endregion
	}
}
