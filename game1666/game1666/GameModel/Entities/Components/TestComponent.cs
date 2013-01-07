/***
 * game1666: TestComponent.cs
 * Copyright Stuart Golodetz, 2012. All rights reserved.
 ***/

using System.Collections.Generic;
using System.Xml.Linq;
using game1666.GameModel.Entities.Base;
using game1666.GameModel.Entities.Interfaces.Components;

namespace game1666.GameModel.Entities.Components
{
	/// <summary>
	/// An instance of this class can be used as a component with which to test entity loading.
	/// We have to put it here rather than in the test project because it will be instantiated
	/// by reflection.
	/// </summary>
	sealed class TestComponent : ModelEntityComponent
	{
		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The group of the component.
		/// </summary>
		public override string Group { get { return ModelEntityComponentGroups.TEST; } }

		/// <summary>
		/// The name of the component.
		/// </summary>
		public override string Name { get { return "Test"; } }

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a blank test component.
		/// </summary>
		public TestComponent()
		{}

		/// <summary>
		/// Constructs a test component directly from its properties.
		/// </summary>
		/// <param name="properties">The properties of the component.</param>
		public TestComponent(IDictionary<string,dynamic> properties)
		:	base(properties)
		{}

		/// <summary>
		/// Constructs a test component from its XML representation.
		/// </summary>
		/// <param name="componentElt">The root element of the component's XML representation.</param>
		/// <param name="fixedProperties">Any component properties that are fixed from code instead of loaded in (can be null).</param>
		public TestComponent(XElement componentElt, IDictionary<string,IDictionary<string,dynamic>> fixedProperties)
		:	base(componentElt, fixedProperties)
		{}

		#endregion
	}
}
